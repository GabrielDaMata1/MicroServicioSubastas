using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Interfaces;
using Infrastructure.Repositories.MongoDB;
using MassTransit;

namespace Infrastructure.Consumers
{
    public class SubastaActivaConsumer:IConsumer<SubastaActivaEvent>
    {
        private readonly ISubastaService _subastaService;

        public SubastaActivaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }

        public async Task Consume(ConsumeContext<SubastaActivaEvent> context)
        {
            var id = context.Message.SubastaId;

            var resul=await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(id, "Active");
            if (resul== HttpStatusCode.OK)
                await _subastaService.ActualizarEstadoSubastaMongoAsync(id, "Active");

        }
    }
}
