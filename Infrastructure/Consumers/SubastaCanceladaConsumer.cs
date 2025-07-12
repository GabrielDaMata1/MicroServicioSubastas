using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Service;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    /// <summary>
    /// Clase consumer que se encarga de consumir el evente SubastaCanceladaEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class SubastaCanceladaConsumer : IConsumer<SubastaCanceladaEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre los jobs en Hangfire, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaSchedule _subastaSchedule;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto en el Microservicio Producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre las notificaciones en el Microservicio Notificaciones, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly INotificacionService _notificacionService;

        public SubastaCanceladaConsumer(ISubastaService subastaService, ISubastaSchedule subastaSchedule, IProductoService productoService, IUsuarioService usuarioService, INotificacionService notificacionService)
        {
            _subastaService = subastaService;
            _subastaSchedule = subastaSchedule;
            _productoService = productoService;
            _usuarioService = usuarioService;
            _notificacionService = notificacionService;
        }

        /// <summary>
        /// Método que se encarga de procesar las operaciones cuando una subasta es cancelada por falta del pago.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID de la subasta cancelada</param>
        /// <exception cref="FalloAlModificarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar el estado de la subasta en la base de datos.
        /// </exception>
        /// <exception cref="FalloAlObtenerSubastasException">
        /// Esta excepcion ocurre si no se pudo obtener la subasta en la base de datos en MongoDB.
        /// </exception>
        /// <exception cref="FalloAlObtenerProductoException">
        /// Esta excepcion ocurre si no se pudo obtener el producto en el Microservicio Producto.
        /// </exception>
        /// <exception cref="FalloAlObtenerUsuarioException">
        /// Esta excepcion ocurre si no se pudo obtener el usuario en el Microservicio Usuarios.
        /// </exception>
        /// <exception cref="FalloAlModificarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<SubastaCanceladaEvent> context)
        {
          try
          {
            var id = context.Message.idSubasta;

            //Se actualiza el estado de la subasta a "Canceled" en la base de datos en PostgreSQL
            var resul = await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(id, "Canceled");

            //En caso de que la modificación falle en la base de datos en PostgreSQL, se lanza la excepción
            if (resul != HttpStatusCode.OK)
                throw new FalloAlModificarSubastaException();

            //Se actualiza el estado de la subasta a "Canceled" en la base de datos en MongoDB
            await _subastaService.ActualizarEstadoSubastaMongoAsync(id, "Canceled");

            //Se obtiene la subasta en la base de datos en MongoDB
            var subasta = await _subastaService.ObtenerSubastaPorIdMongoAsync(id);

              //En caso de que la consulta falle en la base de datos en MongoDB, se lanza la excepción
              if (subasta==null)
                 throw new FalloAlObtenerSubastasException();
            
            //Se obtiene el producto de la subasta desde el Microservicio Producto
            var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

            //En caso de que la consulta falle en el Microservicio Producto, se lanza la excepción
            if (producto == null)
                  throw new FalloAlObtenerProductoException();

            //Se obtiene el ID del subastador de la subasta desde el Microservicio Usuarios
            var idUsuario = await _productoService.ObtenerUsuarioIdPorIdProductoAsync(producto.Id);

            //En caso de que la consulta falle en el Microservicio Usuarios, se lanza la excepción
            if (idUsuario==Guid.Empty)
                  throw new FalloAlObtenerUsuarioException();


            //Se obtiene el correo del subastador de la subasta desde el Microservicio Usuarios
            var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(idUsuario);

            //En caso de que la consulta falle en el Microservicio Usuarios, se lanza la excepción
             if (correoUsuario == null)
                  throw new FalloAlObtenerUsuarioException();


             //Se actualiza el estado del producto en el Microservicio Producto
             var modificarEstadoProducto = await _productoService.ModificarProductoAsync(correoUsuario, producto, "Disponible");

             //En caso de que la modificación falle en el Microservicio Producto, se lanza la excepción
              if (!modificarEstadoProducto)
                  throw new FalloAlModificarProductoException();
              //Se obtiene el ID del usuario ganador de la subasta
              var historialSubasta = await _subastaService.ObtenerHistorialSubastaMongoAsync(context.Message.idSubasta);

              //Se obtiene el correo del usuario ganador de la subasta desde el Microservicio Usuarios
              var correoUsuarioGanador = await _usuarioService.ObtenerCorreoPorIdAsync(historialSubasta.IdUsuario);

              //Se le notifica al usuario y al subastador sobre la cancelación de la subasta
              var notificacionUsuarios = await _notificacionService.EnviarCorreoUsuariosSubastaCancelada(correoUsuario,
                  correoUsuarioGanador, subasta.nombreSubasta.Nombre, producto.NombreProducto.Nombre);

              //En caso de que el envio de correo falle en el Microservicio Notificaciones, se lanza la excepción
              if (!notificacionUsuarios)
                  throw new FalloAlEnviarCorreoException("Ha ocurrido un error al notificar al usuario ganador y al subastador");

          }
          catch (FalloAlModificarSubastaException)
          {
             throw;
          }
          catch (FalloAlObtenerSubastasException)
          {
             throw;
          }
          catch (FalloAlObtenerProductoException)
          {
             throw;
          }
          catch (FalloAlObtenerUsuarioException)
          {
             throw;
          }
          catch (FalloAlEnviarCorreoException)
          {
              throw;
          }
          catch (Exception ex)
          {
             throw new FalloAlModificarSubastaException("Ocurrió un error al modificar la subasta. en la base de datos", ex);
          }



        }
    }
}
