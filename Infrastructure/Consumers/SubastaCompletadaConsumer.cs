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
    public class SubastaCompletadaConsumer : IConsumer<SubastaCompletadaEvent>
    {
        private readonly ISubastaService _subastaService;
        private readonly ISubastaSchedule _subastaSchedule;

        public SubastaCompletadaConsumer(ISubastaService subastaService, ISubastaSchedule subastaSchedule)
        {
            _subastaService = subastaService;
            _subastaSchedule = subastaSchedule;
        }

        public async Task Consume(ConsumeContext<SubastaCompletadaEvent> context)
        {
            var id = context.Message.SubastaId;

            var resul = await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(id, "Completed");
            if (resul == HttpStatusCode.OK)
                await _subastaService.ActualizarEstadoSubastaMongoAsync(id, "Completed");
            await _subastaSchedule.EliminarTemporizadorPagoSubasta(context.Message.SubastaId);

        }
    }
}
