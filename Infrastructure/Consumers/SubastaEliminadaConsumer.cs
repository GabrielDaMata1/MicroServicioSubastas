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
    /// Clase consumer que se encarga de consumir el evento SubastaEliminadaEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class SubastaEliminadaConsumer : IConsumer<SubastaEliminadaEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;

        public SubastaEliminadaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }
        /// <summary>
        /// Método que se encarga de procesar la eliminación de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID de la subasta.</param>
        /// <exception cref="FalloAlEliminarSubastaException">
        /// Esta excepcion ocurre si no se pudo modificar la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<SubastaEliminadaEvent> context)
        {
            try
            {
                //Se elimina la subasta en la base de datos en MongoDB
                await _subastaService.EliminarSubastaMongoAsync(context.Message.idSubasta);
            }
            catch (Exception ex)
            {
                throw new FalloAlEliminarSubastaException("Ocurrió un error al eliminar el estado de la subasta de la base de datos en MongoDB", ex);
            }
        }
    }
}
