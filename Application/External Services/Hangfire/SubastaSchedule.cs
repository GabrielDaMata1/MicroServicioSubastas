using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Hangfire;
using MassTransit;

namespace Application.External_Services.Hangfire
{
    public class SubastaSchedule: ISubastaSchedule
    {
        private readonly IBus _bus;
        private readonly ISubastaJobRepository _subastaJobRepository;

        public SubastaSchedule(IBus bus, ISubastaJobRepository subastaJobRepository)
        {
            _bus = bus;
            _subastaJobRepository = subastaJobRepository;
        }

        public async Task ProgramarEventosDeSubasta(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            var fechaInicioLocal = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Local);

            var fechaFinLocal = DateTime.SpecifyKind(fechaFin, DateTimeKind.Local);


            var jobIdInicio = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarInicio(subastaId, fechaInicioLocal, fechaFinLocal), fechaInicioLocal);

            await _subastaJobRepository.GuardarJobAsync(subastaId, "Inicio", jobIdInicio);

            var jobIdFin = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarFin(subastaId, fechaInicioLocal, fechaFinLocal), fechaFinLocal);

            await _subastaJobRepository.GuardarJobAsync(subastaId, "Fin", jobIdFin);

            var fechaPago = fechaFin.AddDays(5);

            var JobIdPago=BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarTemporizadorPago(subastaId), fechaPago);

            await _subastaJobRepository.GuardarJobAsync(subastaId, "Pago", JobIdPago);


        }

        public async Task ReprogramarEventosDeSubasta(Guid subastaId, DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
        {
            var jobIdInicioAntiguo = await _subastaJobRepository.ObtenerJobIdAsync(subastaId, "Inicio");
            var jobIdFinAntiguo = await _subastaJobRepository.ObtenerJobIdAsync(subastaId, "Fin");
            var jobIdPagoAntiguo = await _subastaJobRepository.ObtenerJobIdAsync(subastaId, "Pago");


            BackgroundJob.Delete(jobIdInicioAntiguo);
            BackgroundJob.Delete(jobIdFinAntiguo);
            BackgroundJob.Delete(jobIdPagoAntiguo);

            var nuevaFechaInicioLocal = DateTime.SpecifyKind(nuevaFechaInicio, DateTimeKind.Local);
            var nuevaFechaFinLocal = DateTime.SpecifyKind(nuevaFechaFin, DateTimeKind.Local);

            var nuevoJobIdInicio = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarInicio(subastaId, nuevaFechaInicioLocal, nuevaFechaFinLocal), nuevaFechaInicioLocal);
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Inicio", nuevoJobIdInicio);

            var nuevoJobIdFin = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarFin(subastaId, nuevaFechaInicioLocal, nuevaFechaFinLocal), nuevaFechaFinLocal);
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Fin", nuevoJobIdFin);

            var fechaPago = nuevaFechaFin.AddDays(5);
            var nuevoJobIdPago = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarTemporizadorPago(subastaId), fechaPago);
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Pago", nuevoJobIdPago);

        }


        public async Task EliminarTemporizadorPagoSubasta(Guid subastaId)
        {
            var jobIdPago = await _subastaJobRepository.ObtenerJobIdAsync(subastaId, "Pago");
            if (!string.IsNullOrEmpty(jobIdPago))
            {
                BackgroundJob.Delete(jobIdPago);
                await _subastaJobRepository.EliminarJobIdAsync(subastaId, "Pago");
            }
        }

        public async Task PublicarInicio(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            await _bus.Publish(new SubastaIniciadaEvent(subastaId, fechaInicio, fechaFin));
        }

        public async Task PublicarFin(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            Console.WriteLine("hola2");
            await _bus.Publish(new SubastaFinalizadaEvent(subastaId, fechaInicio, fechaFin));
        }

        public async Task PublicarTemporizadorPago(Guid subastaId)
        {
            await _bus.Publish(new TemporizadorCancelarPagoEvent(subastaId));
        }


    }
}
