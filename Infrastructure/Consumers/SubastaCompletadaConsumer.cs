using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    /// <summary>
    /// Clase consumer que se encarga de consumir el evento SubastaCompletadaEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class SubastaCompletadaConsumer : IConsumer<SubastaCompletadaEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre los jobs en Hangfire, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaSchedule _subastaSchedule;

        public SubastaCompletadaConsumer(ISubastaService subastaService, ISubastaSchedule subastaSchedule)
        {
            _subastaService = subastaService;
            _subastaSchedule = subastaSchedule;
        }

        /// <summary>
        /// Método que se encarga de procesar la modificación del estado de la subasta tras el pago de la subasta.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID de la subasta.</param>
        /// <exception cref="FalloAlModificarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<SubastaCompletadaEvent> context)
        {
            try
            {
                var id = context.Message.SubastaId;

                //Se actualiza el estado de la subasta a "Completed" en la base de datos en PostgreSQL
                var resul = await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(id, "Completed");

                if (resul == HttpStatusCode.OK)
                    //Se actualiza el estado de la subasta a "Completed" en la base de datos en MongoDB
                    await _subastaService.ActualizarEstadoSubastaMongoAsync(id, "Completed");
                //Se elimina el job que controlaba el temporizador del pago de la subasta
                await _subastaSchedule.EliminarTemporizadorPagoSubasta(context.Message.SubastaId);
            }
            catch (Exception ex)
            {
                throw new FalloAlModificarSubastaException("Ocurrió un error al modificar el estado de la subasta de la base de datos en MongoDB", ex);
            }

        }
    }
}
