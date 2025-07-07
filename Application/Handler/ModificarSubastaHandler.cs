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
    /// <summary>
    /// Clase Handler que se encarga de modificar una subasta en específico antes de que comience.
    /// </summary>
    public class ModificarSubastaHandler : IRequestHandler<ModificarSubastaCommand, bool>
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
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre los jobs en Hangfire, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaSchedule _subastaSchedule;

        public ModificarSubastaHandler(ISubastaService subastaService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService, ISubastaSchedule subastaSchedule)
        {
            _publishEndpoint = publishEndpoint;
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _subastaSchedule= subastaSchedule;
        }

        /// <summary>
        /// Metodo que se encarga de procesar la modificación de una subasta.
        /// </summary>
        /// <param name="request">Parametro DTO que contiene el correo del subastador y la información de la subasta a modificar.</param>
        /// <returns>Retorna un valor booleano True si todas las operaciones fueron exitosas.</returns>
        /// <exception cref="SubastaNoModificableException">
        /// Esta excepcion ocurre si la subasta ya ha comenzado.
        /// </exception>
        /// <exception cref="FalloAlModificarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar en las bases de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<bool> Handle(ModificarSubastaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Se obtiene la subasta a modificar en la base de datos en MongoDB
                var subastaBD = await _subastaService.ObtenerSubastaPorIdMongoAsync(request.subastaDto.Id);

                //En caso de que la subasta ya haya empezado, se lanza la excepción
                if (subastaBD.fechaInicioSubasta.fechaInicio <= DateTime.UtcNow)
                    throw new SubastaNoModificableException();

                //Se obtiene el ID del subastador que organizó la subasta desde el Microservicio Usuarios
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.subastaDto.correoUsuario);

                //Se crea la instancia del objeto Subasta
                var subasta = SubastaFactory.CrearSubastaConId(request.subastaDto.Id, request.subastaDto.Nombre, request.subastaDto.Descripcion, request.subastaDto.idProducto, request.subastaDto.fechaInicio, 
                    request.subastaDto.fechaFin, request.subastaDto.incrementoMinimo, request.subastaDto.precioReserva, request.subastaDto.estado);

                //Se modifica la subasta en la base de datos de PostgreSQL
                var subastaModificada = await _subastaService.ModificarSubastaPostgreSQLAsync(subasta, idUsuario);

                //En caso de que ocurra un error al modificar en la base de datos en PostgreSQL, se lanza una excepción
                if (subastaModificada !=HttpStatusCode.OK)
                    throw new FalloAlModificarSubastaException("No se pudo modificar la subasta en la base de datos PostgreSQL. ");

                //Se publica el mensaje en la cola de RabbitMQ para sincronizar la base de datos de MongoDB con PostgreSQL
                await _publishEndpoint.Publish(new SubastaModificadaEvent(subasta, idUsuario));

                //Se reprograma la fecha de inicio y la fecha fin de la subasta en Hangfire
                await _subastaSchedule.ReprogramarEventosDeSubasta(request.subastaDto.Id,request.subastaDto.fechaInicio,request.subastaDto.fechaFin);
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
