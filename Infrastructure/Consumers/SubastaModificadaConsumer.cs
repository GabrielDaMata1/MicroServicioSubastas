using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    public class SubastaModificadaConsumer : IConsumer<SubastaModificadaEvent>
    {
        private readonly ISubastaService _subastaService;

        public SubastaModificadaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }
        public async Task Consume(ConsumeContext<SubastaModificadaEvent> context)
        {
            await _subastaService.ModificarSubastaMongoAsync(context.Message.subasta, context.Message.idUsuario);

        }
    }
}
