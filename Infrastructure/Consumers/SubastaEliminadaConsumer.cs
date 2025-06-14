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
    public class SubastaEliminadaConsumer : IConsumer<SubastaEliminadaEvent>
    {
        private readonly ISubastaService _subastaService;

        public SubastaEliminadaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }
        public async Task Consume(ConsumeContext<SubastaEliminadaEvent> context)
        {
            await _subastaService.EliminarSubastaMongoAsync(context.Message.idSubasta);

        }
    }
}
