using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exceptions;
using Application.External_Services.Hangfire;
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
    /// Clase Handler que se encarga de registrar una nueva subasta que organiza un subastador.
    /// </summary>
    public class RegistrarSubastaHandler : IRequestHandler<RegistrarSubastaCommand, bool>
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
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre los jobs en Hangfire, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaSchedule _subastaSchedule;

        public RegistrarSubastaHandler(ISubastaService subastaService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService, IProductoService productoService, ISubastaSchedule subastaSchedule)
        {
            _publishEndpoint = publishEndpoint;
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
            _subastaSchedule= subastaSchedule;
        }

        /// <summary>
        /// Metodo que se encarga de procesar el registro de una nueva subasta.
        /// </summary>
        /// <param name="request">Parametro DTO que contiene el correo del subastador y la información de la subasta a registrar.</param>
        /// <returns>Retorna un valor booleano True si todas las operaciones fueron exitosas.</returns>
        /// <exception cref="SubastaNoModificableException">
        /// Esta excepcion ocurre si la subasta ya ha comenzado.
        /// </exception>
        /// <exception cref="FalloAlModificarProductoException">
        /// Esta excepcion ocurre si ocurrió un error al modificar el producto en el Microservicio Producto.
        /// </exception>
        /// <exception cref="FalloAlModificarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar en las bases de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<bool> Handle(RegistrarSubastaCommand request, CancellationToken cancellationToken)
        {
            try
            {

                //Se obtiene el ID del subastador que organiza la subasta desde el Microservicio Usuarios
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.SubastaDto.correoUsuario);

                //Se obtiene el ID del usuario que registró el producto a subastar desde el Microservicio Producto
                var idUsuarioProducto = await _productoService.ObtenerUsuarioIdPorIdProductoAsync(request.SubastaDto.idProducto);

                //Se obtiene el producto que se está subastando desde el Microservicio de Producto
                var producto = await _productoService.ObtenerProductoPorGuid(request.SubastaDto.idProducto);

                //En caso de que el producto no le pertenezca al usuario autenticado, se lanza la excepción
                if (idUsuario != idUsuarioProducto)
                    throw new ProductoNoPerteneceAlUsuarioException();

                //En caso de que el producto se esté subastando, se lanza la excepción
                if (producto.EstadoProducto.estadoProducto.Equals("Subastando"))
                    throw new ProductoYaSubastandoException();

                //Se crea la instancia del objeto Subasta
                var subasta = SubastaFactory.CrearSubasta(request.SubastaDto.Nombre, request.SubastaDto.Descripcion, request.SubastaDto.idProducto, request.SubastaDto.fechaInicio, request.SubastaDto.fechaFin,
                    request.SubastaDto.incrementoMinimo, request.SubastaDto.precioReserva, "Pending");

                //Se registra la subasta en la base de datos en PostgreSQL
                var subastaId = await _subastaService.RegistrarSubastaPostgreSQLAsync(subasta, idUsuario);

                //En caso de que ocurra un error al registrar en la base de datos en PostgreSQL, se lanza una excepción
                if (subastaId == Guid.Empty)
                    throw new FalloAlRegistrarSubastaException("No se pudo registrar la subasta en la base de datos PostgreSQL. ");


                //Se modifica el estado del producto que se está subastando desde el Microservicio de Producto
                var modificarEstadoProducto = await _productoService.ModificarProductoAsync(request.SubastaDto.correoUsuario, producto,"Subastando");

                //En caso de que la modificación falle, se lanza la excepción
                if (!modificarEstadoProducto)
                    throw new FalloAlModificarProductoException();

                //Se publica el mensaje en la cola de RabbitMQ para sincronizar la base de datos de MongoDB con PostgreSQL
                await _publishEndpoint.Publish(new SubastaRegistradaEvent(subasta, idUsuario));

                //Se programa la fecha de inicio y la fecha fin de la subasta en Hangfire
                await _subastaSchedule.ProgramarEventosDeSubasta(subastaId,request.SubastaDto.fechaInicio,request.SubastaDto.fechaFin);



                return true;
            }
            catch (ProductoNoPerteneceAlUsuarioException)
            {
                throw;
            }
            catch (ProductoYaSubastandoException)
            {
                throw;
            }
            catch (FalloAlRegistrarSubastaException)
            {
                throw;
            }
            catch (FalloAlModificarProductoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FalloAlRegistrarSubastaException("Ocurrió un error al registrar la subasta. en la base de datos", ex);
            }
        }
    }
}
