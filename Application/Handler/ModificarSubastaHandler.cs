using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.Exceptions;
using Application.Service;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    public class ModificarSubastaHandler : IRequestHandler<ModificarSubastaCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISubastaService _subastaService;
        private readonly IUsuarioService _usuarioService;

        public ModificarSubastaHandler(ISubastaService subastaService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService)
        {
            _publishEndpoint = publishEndpoint;
            _subastaService = subastaService;
            _usuarioService = usuarioService;
        }
        public async Task<bool> Handle(ModificarSubastaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subastaBD = await _subastaService.ObtenerSubastaPorIdMongoAsync(request.subastaDto.Id);
                if (subastaBD.fechaInicioSubasta.fechaInicio <= DateTime.UtcNow)
                    throw new SubastaNoModificableException();

                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.subastaDto.correoUsuario);

                var subasta = SubastaFactory.CrearSubastaConId(
                    request.subastaDto.Id,
                    request.subastaDto.Nombre,
                    request.subastaDto.Descripcion,
                    request.subastaDto.idProducto,
                    request.subastaDto.fechaInicio,
                    request.subastaDto.fechaFin,
                    request.subastaDto.incrementoMinimo,
                    request.subastaDto.precioReserva
                );

                var subastaModificada = await _subastaService.ModificarSubastaPostgreSQLAsync(subasta, idUsuario);

                if (subastaModificada !=HttpStatusCode.OK)
                    throw new FalloAlModificarSubastaException("No se pudo modificar la subasta en la base de datos PostgreSQL. ");

                await _publishEndpoint.Publish(new SubastaModificadaEvent(subasta, idUsuario));
                return true;
            }
            catch (SubastaNoModificableException)
            {
                throw;
            }
            catch (FalloAlModificarSubastaException)
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
