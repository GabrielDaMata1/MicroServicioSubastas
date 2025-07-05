using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    public class EntregaPremioConfirmadaConsumer : IConsumer<EntregaPremioConfirmadaEvent>
    {
        private readonly ISubastaService _subastaService;

        public EntregaPremioConfirmadaConsumer(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }

        public async Task Consume(ConsumeContext<EntregaPremioConfirmadaEvent> context)
        {
            var id = context.Message.idSubasta;

            var resul = await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(id, "Delivered");
            if (resul == HttpStatusCode.OK)
                await _subastaService.ActualizarEstadoSubastaMongoAsync(id, "Delivered");

        }
    }
}
