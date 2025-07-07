using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    /// <summary>
    /// Clase consumer que se encarga de consumir el evento SubastaAcabadaEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class SubastaAcabadaConsumer : IConsumer<SubastaAcabadaEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre las pujas en el Microservicio Pujas, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IPujaService _pujaService;
        /// <summary>
        /// Atributo que corresponde a las publicación de mensajes a la cola de RabbitMQ.
        /// </summary>
        private readonly IPublishEndpoint _publish;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto en el Microservicio Producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;



        public SubastaAcabadaConsumer(ISubastaService subastaService, IPublishEndpoint publish, IPujaService pujaService, IProductoService productoService, IUsuarioService usuarioService)
        {
            _subastaService = subastaService;
            _publish = publish;
            _pujaService = pujaService;
            _productoService = productoService;
            _usuarioService= usuarioService;
        }

        /// <summary>
        /// Método que se encarga de procesar las operaciones cuando acaba una subasta.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID de la subasta finalizada</param>
        /// <exception cref="FalloAlRegistrarHistorialSubastaException">
        /// Esta excepcion ocurre si no se pudo registrar el historial de la subasta en las bases de datos.
        /// </exception>
        /// <exception cref="FalloAlModificarProductoException">
        /// Esta excepcion ocurre si no se pudo modificar el estado del producto en el Microservicio Producto.
        /// </exception>
        /// <exception cref="FalloAlModificarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<SubastaAcabadaEvent> context)
        {
            try
            {

                //Se obtiene la subasta que finalizó en la base de datos en MongoDB
                var subasta = await _subastaService.ObtenerSubastaPorIdMongoAsync(context.Message.SubastaId);

                //Se obtiene el producto que se subastó desde el Microservicio Producto
                var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

                //Se obtiene el ID del subtastador que organizó la subastas desde el Microservicio Usuarios
                var idUsuario = await _productoService.ObtenerUsuarioIdPorIdProductoAsync(producto.Id);

                //Se obtiene el correo del subtastador que organizó la subastas desde el Microservicio Usuarios
                var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(idUsuario);


                //Se actualiza el estado de la subasta a "Ended" en la base de datos en PostgreSQL
                var resul = await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(context.Message.SubastaId, "Ended");

                if (resul == HttpStatusCode.OK)

                    //Se actualiza el estado de la subasta a "Ended" en la base de datos en MongoDB
                    await _subastaService.ActualizarEstadoSubastaMongoAsync(context.Message.SubastaId, "Ended");

                //Se obtiene la puja ganadora de la subasta desde el Microservicio de Pujas
                var pujaGanadora = await _pujaService.ObtenerPujaGanadoraPorIdSubasta(context.Message.SubastaId);


                if (pujaGanadora == null)
                {
                    //Se actualiza el estado de la subasta a "Deserted" en la base de datos en PostgreSQL
                    var subastaDesierta = await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(context.Message.SubastaId, "Deserted");

                    if (subastaDesierta == HttpStatusCode.OK)
                        //Se actualiza el estado de la subasta a "Deserted" en la base de datos en MongoDB
                        await _subastaService.ActualizarEstadoSubastaMongoAsync(context.Message.SubastaId, "Deserted");

                    //Se actualiza el estado del producto "Subastado" desde el Microservicio Producto
                    var modificarEstadoProducto = await _productoService.ModificarProductoAsync(correoUsuario, producto, "Disponible");

                    return;
                }
                //Se crea una instancia del objeto HistorialSubasta
                var historialSubasta = HistorialSubastaFactory.CrearHistorialSubasta(pujaGanadora.IdUsuario,
                    pujaGanadora.IdSubasta, pujaGanadora.MontoPuja.montoPuja);

                if (pujaGanadora.MontoPuja.montoPuja < subasta.precioReservaSubasta.precioReserva)
                {
                    //Se registra el historial de subasta en la base de datos en PostgreSQL
                    var idHistorialSubastaNoGanadora = await _subastaService.RegistrarHistorialSubastaPostgreSQLAsync(historialSubasta,"No Ganador");

                    //En caso de que el registro falle en la base de datos en PostgreSQL, se lanza la excepción
                    if (idHistorialSubastaNoGanadora == Guid.Empty)
                        throw new FalloAlRegistrarHistorialSubastaException();

                    //Se registra el historial de subasta en la base de datos en MongoDB
                    await _subastaService.RegistrarHistorialSubastaMongoAsync(historialSubasta,"No Ganador");

                    //Se actualiza el estado del producto "Disponible" desde el Microservicio Producto
                    var modificarEstadoProducto = await _productoService.ModificarProductoAsync(correoUsuario, producto, "Disponible");

                    //En caso de que la modificación falle en el Microservicio Producto, se lanza la excepción
                    if (!modificarEstadoProducto)
                        throw new FalloAlModificarProductoException();
                }

                if (pujaGanadora.MontoPuja.montoPuja > subasta.precioReservaSubasta.precioReserva)
                {
                    //Se registra el historial de subasta en la base de datos en PostgreSQL
                    var idHistorialSubastaGanadora = await _subastaService.RegistrarHistorialSubastaPostgreSQLAsync(historialSubasta, "Ganador");

                    //En caso de que el registro falle en la base de datos en PostgreSQL, se lanza la excepción
                    if (idHistorialSubastaGanadora == Guid.Empty)
                        throw new FalloAlRegistrarHistorialSubastaException();

                    //Se registra el historial de subasta en la base de datos en MongoDB
                    await _subastaService.RegistrarHistorialSubastaMongoAsync(historialSubasta, "Ganador");

                    //Se actualiza el estado del producto "Subastado" desde el Microservicio Producto
                    var modificarEstadoProducto = await _productoService.ModificarProductoAsync(correoUsuario, producto, "Subastado");

                    //En caso de que la modificación falle en el Microservicio Producto, se lanza la excepción
                    if (!modificarEstadoProducto)
                        throw new FalloAlModificarProductoException();
                }

            }
            catch (FalloAlRegistrarHistorialSubastaException)
            {
                throw;
            }
            catch (FalloAlModificarProductoException)
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
