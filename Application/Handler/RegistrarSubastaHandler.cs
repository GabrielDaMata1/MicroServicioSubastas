using System;
using System.Collections.Generic;
using System.Linq;
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
    public class RegistrarSubastaHandler : IRequestHandler<RegistrarSubastaCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISubastaService _subastaService;
        private readonly IUsuarioService _usuarioService;

        public RegistrarSubastaHandler(ISubastaService subastaService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService)
        {
            _publishEndpoint = publishEndpoint;
            _subastaService = subastaService;
            _usuarioService = usuarioService;
        }

        public async Task<bool> Handle(RegistrarSubastaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.SubastaDto.correoUsuario);

                var subasta = SubastaFactory.CrearSubasta(
                    request.SubastaDto.Nombre,
                    request.SubastaDto.Descripcion,
                    request.SubastaDto.idProducto,
                    request.SubastaDto.fechaInicio,
                    request.SubastaDto.fechaFin,
                    request.SubastaDto.incrementoMinimo,
                    request.SubastaDto.precioReserva
                );

                var subastaId = await _subastaService.RegistrarSubastaPostgreSQLAsync(subasta, idUsuario);

                if (subastaId == Guid.Empty)
                    throw new FalloAlRegistrarSubastaException("No se pudo registrar la subasta en la base de datos PostgreSQL. ");

                await _publishEndpoint.Publish(new SubastaRegistradaEvent(subasta, idUsuario));
                return true;
            }
            catch (FalloAlRegistrarSubastaException)
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
