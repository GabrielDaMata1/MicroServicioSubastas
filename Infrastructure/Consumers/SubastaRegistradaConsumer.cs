using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Service;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    /// <summary>
    /// Clase consumer que se encarga de consumir el evento SubastaRegistradaEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class SubastaRegistradaConsumer : IConsumer<SubastaRegistradaEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;

        public SubastaRegistradaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }

        /// <summary>
        /// Método que se encarga de procesar el registro  de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID del subastador y un objeto Subasta con su detalle.</param>
        /// <exception cref="FalloAlRegistrarSubastaException">
        /// Esta excepcion ocurre si no se pudo registrar la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<SubastaRegistradaEvent> context)
        {
            try
            {
                //Se registra la subasta en la base de datos en MongoDB
                await _subastaService.RegistrarSubastaMongoAsync(context.Message.subasta, context.Message.idUsuario);
            }
            catch (Exception ex)
            {
                throw new FalloAlRegistrarSubastaException("Ocurrió un error al registrar la subasta de la base de datos en MongoDB", ex);
            }
        }
    }
}
