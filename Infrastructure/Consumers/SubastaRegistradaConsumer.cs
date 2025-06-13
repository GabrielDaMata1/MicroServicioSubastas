using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Service;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    public class SubastaRegistradaConsumer : IConsumer<SubastaRegistradaEvent>
    {
        private readonly ISubastaService _subastaService;

        public SubastaRegistradaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }
        public async Task Consume(ConsumeContext<SubastaRegistradaEvent> context)
        {
            await _subastaService.RegistrarProductoMongoAsync(context.Message.subasta, context.Message.idUsuario);

        }
    }
}
