using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.Exceptions;
using Application.Service;
using Domain.Entities;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    /// <summary>
    /// Clase Handler que se encarga de eliminar una subasta en específico antes de que comience.
    /// </summary>
    public class EliminarSubastaHandler : IRequestHandler<EliminarSubastaCommand, bool>
    {
        /// <summary>
        /// Atributo que corresponde a las publicación de mensajes a la cola de RabbitMQ.
        /// </summary>
        private readonly IPublishEndpoint _publishEndpoint;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto en el Microservicio Producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;

        public EliminarSubastaHandler(ISubastaService subastaService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService, IProductoService productoService)
        {
            _publishEndpoint = publishEndpoint;
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
        }
        /// <summary>
        /// Metodo que se encarga de procesar la eliminación de una subasta.
        /// </summary>
        /// <param name="request">Parametro DTO que contiene el correo del subastador y el ID de la subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano True si todas las operaciones fueron exitosas.</returns>
        /// <exception cref="SubastaNoPertenceAlUsuarioException">
        /// Esta excepcion ocurre si el usuario autenticado no es el dueño de la subasta.
        /// </exception>
        /// <exception cref="SubastaNoEliminableException">
        /// Esta excepcion ocurre si la subasta ya ha comenzado.
        /// </exception>
        /// <exception cref="FalloAlModificarProductoException">
        /// Esta excepcion ocurre si ocurrió un error al modificar el producto en el Microservicio Producto.
        /// </exception>
        /// <exception cref="FalloAlEliminarSubastaException">
        /// Esta excepcion ocurre si no se pudo eliminar en las bases de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<bool> Handle(EliminarSubastaCommand request, CancellationToken cancellationToken)
        {
            try
            {

                //Se obtiene el ID del subastador que organizó la subasta desde el Microservicio Usuarios
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.subastaDto.correoUsuario);

                // Se obtiene la subasta a eliminar en la base de datos en MongoDB
                var subastaBD = await _subastaService.ObtenerSubastaPorIdMongoAsync(request.subastaDto.idSubasta);

                //Se obtiene el ID del subastador que organizó la subasta
                var usuarioIDSubasta = await _subastaService.ObtenerUsuarioIdPorSubastaIdMongoAsync(request.subastaDto.idSubasta);

                //En caso de que la subasta no le pertenezca al usuario autenticado, se lanza la excepción
                if (idUsuario != usuarioIDSubasta)
                    throw new SubastaNoPertenceAlUsuarioException();

                //En caso de que la subasta ya haya empezado, se lanza la excepción
                if (subastaBD.fechaInicioSubasta.fechaInicio <= DateTime.UtcNow)
                    throw new SubastaNoEliminableException();

                //Se elimina la subasta en la base de datos en PostgreSQL
                var subastaEliminada = await _subastaService.EliminarSubastaPostgreSQLAsync(request.subastaDto.idSubasta);

                //En caso de que ocurra un error al eliminar en la base de datos en PostgreSQL, se lanza una excepción
                if (!subastaEliminada)
                    throw new FalloAlEliminarSubastaException("No se pudo eliminar la subasta en la base de datos PostgreSQL. ");

                //Se obtiene el producto que se está subastando desde el Microservicio de Producto
                var producto = await _productoService.ObtenerProductoPorGuid(subastaBD.idProductoSubasta);

                //Se modifica el estado del producto que se está subastando desde el Microservicio de Producto
                var modificarEstadoProducto = await _productoService.ModificarProductoDisponibleAsync(request.subastaDto.correoUsuario, producto);

                //En caso de que la modificación falle, se lanza la excepción
                if (!modificarEstadoProducto)
                    throw new FalloAlModificarProductoException();


                //Se publica el mensaje en la cola de RabbitMQ para sincronizar la base de datos de MongoDB con PostgreSQL
                await _publishEndpoint.Publish(new SubastaEliminadaEvent(request.subastaDto.idSubasta));
                return true;
            }
            catch (SubastaNoPertenceAlUsuarioException)
            {
                throw;
            }
            catch (SubastaNoEliminableException)
            {
                throw;
            }
            catch (FalloAlEliminarSubastaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FalloAlEliminarSubastaException("Ocurrió un error al eliminar la subasta. en la base de datos", ex);
            }
        }
    }
}
