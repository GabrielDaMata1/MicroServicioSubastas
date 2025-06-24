using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Interfaces;
using Hangfire;
using MassTransit;

namespace Application.External_Services.Hangfire
{
    public class SubastaSchedule: ISubastaSchedule
    {
        private readonly IBus _bus;

        public SubastaSchedule(IBus bus)
        {
            _bus = bus;
        }

        public void ProgramarEventosDeSubasta(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarInicio(subastaId, fechaInicio, fechaFin), fechaInicio);
            BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarFin(subastaId, fechaInicio, fechaFin), fechaFin);
        }

        public void ReprogramarEventosDeSubasta(Guid subastaId, DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
        {
            var jobIdInicio = $"subasta-inicio-{subastaId}";
            var jobIdFin = $"subasta-fin-{subastaId}";

            BackgroundJob.Delete(jobIdInicio);
            BackgroundJob.Delete(jobIdFin);

            BackgroundJob.Schedule<SubastaSchedule>(jobIdInicio, s => s.PublicarInicio(subastaId, nuevaFechaInicio, nuevaFechaFin), nuevaFechaInicio);

            BackgroundJob.Schedule<SubastaSchedule>(jobIdFin, s => s.PublicarFin(subastaId, nuevaFechaInicio, nuevaFechaFin), nuevaFechaFin);
        }


        public async Task PublicarInicio(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            await _bus.Publish(new SubastaIniciadaEvent(subastaId, fechaInicio, fechaFin));
        }

        public async Task PublicarFin(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            await _bus.Publish(new SubastaFinalizadaEvent(subastaId, fechaInicio, fechaFin));
        }

    }
}
