using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    /// <summary>
    /// Clase consumer que se encarga de consumir el evento SubastaModificadaEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class SubastaModificadaConsumer : IConsumer<SubastaModificadaEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;

        public SubastaModificadaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }
        /// <summary>
        /// Método que se encarga de procesar la modificación  de una subasta.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID del subastador y un objeto Subasta con su detalle.</param>
        /// <exception cref="FalloAlModificarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<SubastaModificadaEvent> context)
        {
            try
            {
                await _subastaService.ModificarSubastaMongoAsync(context.Message.subasta, context.Message.idUsuario);
            }
            catch (Exception ex)
            {
                throw new FalloAlModificarSubastaException("Ocurrió un error al modificar la subasta de la base de datos en MongoDB", ex);
            }
        }
    }
}
