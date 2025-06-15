using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
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
    public class RegistrarSubastaHandler : IRequestHandler<RegistrarSubastaCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISubastaService _subastaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProductoService _productoService;

        public RegistrarSubastaHandler(ISubastaService subastaService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService, IProductoService productoService)
        {
            _publishEndpoint = publishEndpoint;
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
        }

        public async Task<bool> Handle(RegistrarSubastaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.SubastaDto.correoUsuario);

                var idUsuarioProducto = await _productoService.ObtenerUsuarioIdPorIdProductoAsync(request.SubastaDto.idProducto);

                var producto = await _productoService.ObtenerProductoPorGuid(request.SubastaDto.idProducto);




               if (idUsuario != idUsuarioProducto)
                    throw new ProductoNoPerteneceAlUsuarioException();
               
                if (producto.EstadoProducto.estadoProducto.Equals("Subastando"))
                    throw new ProductoYaSubastandoException();

                var subasta = SubastaFactory.CrearSubasta(
                    request.SubastaDto.Nombre,
                    request.SubastaDto.Descripcion,
                    request.SubastaDto.idProducto,
                    request.SubastaDto.fechaInicio,
                    request.SubastaDto.fechaFin,
                    request.SubastaDto.incrementoMinimo,
                    request.SubastaDto.precioReserva,
                    "Pending"
                );

                var subastaId = await _subastaService.RegistrarSubastaPostgreSQLAsync(subasta, idUsuario);

                if (subastaId == Guid.Empty)
                    throw new FalloAlRegistrarSubastaException("No se pudo registrar la subasta en la base de datos PostgreSQL. ");


                var modificarEstadoProducto = await _productoService.ModificarProductoAsync(request.SubastaDto.correoUsuario, producto);

                if (!modificarEstadoProducto)
                    throw new FalloAlModificarProductoException();

                await _publishEndpoint.Publish(new SubastaRegistradaEvent(subasta, idUsuario));
                return true;
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
