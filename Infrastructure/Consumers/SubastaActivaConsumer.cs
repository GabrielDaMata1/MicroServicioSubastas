using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Events;
using Domain.Interfaces;
using Infrastructure.Repositories.MongoDB;
using MassTransit;

namespace Infrastructure.Consumers
{
    /// <summary>
    /// Clase consumer que se encarga de consumir el evento SubastaActivaEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class SubastaActivaConsumer:IConsumer<SubastaActivaEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;

        public SubastaActivaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }

        /// <summary>
        /// Método que se encarga de procesar la modificación del estado de la subasta tras la activación de la subasta.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID de la subasta.</param>
        /// <exception cref="FalloAlModificarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<SubastaActivaEvent> context)
        {
          try
          { 
            var id = context.Message.SubastaId;

            //Se actualiza el estado de la subasta a "Active" en la base de datos en PostgreSQL
            var resul =await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(id, "Active");

            if (resul== HttpStatusCode.OK)

                //Se actualiza el estado de la subasta a "Delivered" en la base de datos en MongoDB
                await _subastaService.ActualizarEstadoSubastaMongoAsync(id, "Active");
          }
          catch (Exception ex)
          {
              throw new FalloAlModificarSubastaException("Ocurrió un error al modificar el estado de la subasta de la base de datos en MongoDB", ex);
          }

        }
    }
}
