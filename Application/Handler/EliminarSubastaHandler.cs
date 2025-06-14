using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.Exceptions;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    public class EliminarSubastaHandler : IRequestHandler<EliminarSubastaCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISubastaService _subastaService;
        private readonly IUsuarioService _usuarioService;

        public EliminarSubastaHandler(ISubastaService subastaService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService)
        {
            _publishEndpoint = publishEndpoint;
            _subastaService = subastaService;
            _usuarioService = usuarioService;
        }
        public async Task<bool> Handle(EliminarSubastaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.subastaDto.correoUsuario);
                var subastaBD = await _subastaService.ObtenerSubastaPorIdMongoAsync(request.subastaDto.idSubasta);
                var usuarioIDSubasta =
                    await _subastaService.ObtenerUsuarioIdPorSubastaIdMongoAsync(request.subastaDto.idSubasta);

                if (idUsuario != usuarioIDSubasta)
                    throw new SubastaNoPertenceAlUsuarioException();

                if (subastaBD.fechaInicioSubasta.fechaInicio <= DateTime.UtcNow)
                    throw new SubastaNoEliminableException();

                var subastaEliminada =
                    await _subastaService.EliminarSubastaPostgreSQLAsync(request.subastaDto.idSubasta);

                if (!subastaEliminada)
                    throw new FalloAlEliminarSubastaException("No se pudo eliminar la subasta en la base de datos PostgreSQL. ");

                await _publishEndpoint.Publish(new SubastaEliminadaEvent(request.subastaDto.idSubasta));
                return true;
            }
            catch (SubastaNoPertenceAlUsuarioException)
            {
                throw;
            }
            catch (SubastaNoModificableException)
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
