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
    /// <summary>
    /// Clase external service que se encarga de programar, reprogramar y eliminar jobs con Hangfire, y publicar eventos a la cola de RabbitMQ
    /// </summary>

    public class SubastaSchedule: ISubastaSchedule
    {
        /// <summary>
        /// Atributo que corresponde al bus de eventos utilizado para publicar notificaciones de inicio, fin y temporizador de pago de subastas.
        /// </summary>
        private readonly IBus _bus;
        /// <summary>
        /// Atributo que corresponde al repositorio encargado de almacenar y recuperar los identificadores de los jobs en MongoDB.
        /// </summary>

        private readonly ISubastaJobRepository _subastaJobRepository;

        public SubastaSchedule(IBus bus, ISubastaJobRepository subastaJobRepository)
        {
            _bus = bus;
            _subastaJobRepository = subastaJobRepository;
        }

        /// <summary>
        /// Método que se encarga de programar los eventos de inicio, fin y temporizador de pago para una subasta.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        /// <param name="fechaInicio">Parámetro que corresponde a la fecha inicio de la subasta.</param>
        /// <param name="fechaFin">Parámetro que corresponde a la fecha fin de la subasta.</param>
        public async Task ProgramarEventosDeSubasta(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            //Se convierte la fecha de inicio recibida a la fecha local
            var fechaInicioLocal = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Local);

            //Se convierte la fecha fin recibida a la fecha local
            var fechaFinLocal = DateTime.SpecifyKind(fechaFin, DateTimeKind.Local);

            //Se programa la fecha de inicio de la subasta con Hangfire
            var jobIdInicio = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarInicio(subastaId, fechaInicioLocal, fechaFinLocal), fechaInicioLocal);

            //Se registra la información del job de inicio en la base de datos en MongoBD 
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Inicio", jobIdInicio);

            //Se programa la fecha fin de la subasta con Hangfire
            var jobIdFin = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarFin(subastaId, fechaInicioLocal, fechaFinLocal), fechaFinLocal);

            //Se registra la información del job de fin en la base de datos en MongoBD 
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Fin", jobIdFin);

            //Se añaden cinco días para realizar el pago de la subasta
            var fechaPago = fechaFin.AddDays(5);

            //Se programa la fecha de cancelación de la subasta con Hangfire
            var JobIdPago =BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarTemporizadorPago(subastaId), fechaPago);

            //Se registra la información del job de cancelación en la base de datos en MongoBD 
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Pago", JobIdPago);


        }

        /// <summary>
        /// Método que se encarga de reprogramar los eventos de inicio, fin y temporizador de pago para una subasta.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        /// <param name="nuevaFechaInicio">Parámetro que corresponde a la nueva fecha inicio de la subasta.</param>
        /// <param name="nuevaFechaFin">Parámetro que corresponde a la nueva fecha fin de la subasta.</param>
        public async Task ReprogramarEventosDeSubasta(Guid subastaId, DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
        {
            //Se obtiene el ID del job de inicio de la base de datos en MongoDB
            var jobIdInicioAntiguo = await _subastaJobRepository.ObtenerJobIdAsync(subastaId, "Inicio");

            //Se obtiene el ID del job de fin de la base de datos en MongoDB
            var jobIdFinAntiguo = await _subastaJobRepository.ObtenerJobIdAsync(subastaId, "Fin");

            //Se obtiene el ID del job de cancelación de pago de la base de datos en MongoDB
            var jobIdPagoAntiguo = await _subastaJobRepository.ObtenerJobIdAsync(subastaId, "Pago");


            BackgroundJob.Delete(jobIdInicioAntiguo);
            BackgroundJob.Delete(jobIdFinAntiguo);
            BackgroundJob.Delete(jobIdPagoAntiguo);

            //Se convierte la fecha de inicio recibida a la fecha local
            var nuevaFechaInicioLocal = DateTime.SpecifyKind(nuevaFechaInicio, DateTimeKind.Local);

            //Se convierte la fecha fin recibida a la fecha local
            var nuevaFechaFinLocal = DateTime.SpecifyKind(nuevaFechaFin, DateTimeKind.Local);

            //Se reprograma la fecha de inicio de la subasta con Hangfire
            var nuevoJobIdInicio = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarInicio(subastaId, nuevaFechaInicioLocal, nuevaFechaFinLocal), nuevaFechaInicioLocal);

            //Se registra la información del nuevo job de inicio en la base de datos en MongoBD 
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Inicio", nuevoJobIdInicio);

            //Se reprograma la fecha fin de la subasta con Hangfire
            var nuevoJobIdFin = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarFin(subastaId, nuevaFechaInicioLocal, nuevaFechaFinLocal), nuevaFechaFinLocal);

            //Se registra la información del nuevo job fin en la base de datos en MongoBD 
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Fin", nuevoJobIdFin);

            //Se añaden cinco días para realizar el pago de la subasta
            var fechaPago = nuevaFechaFin.AddDays(5);

            //Se reprograma la fecha de cancelación de la subasta con Hangfire
            var nuevoJobIdPago = BackgroundJob.Schedule<SubastaSchedule>(s => s.PublicarTemporizadorPago(subastaId), fechaPago);

            //Se registra la información del nuevo job de cancelación en la base de datos en MongoBD 
            await _subastaJobRepository.GuardarJobAsync(subastaId, "Pago", nuevoJobIdPago);

        }

        /// <summary>
        /// Método que se encarga de eliminar el temporizador de cancelación de una subasta por la falta de pago.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        public async Task EliminarTemporizadorPagoSubasta(Guid subastaId)
        {
            //Se obtiene el ID del job de cancelación de pago de la base de datos en MongoDB
            var jobIdPago = await _subastaJobRepository.ObtenerJobIdAsync(subastaId, "Pago");
            if (!string.IsNullOrEmpty(jobIdPago))
            {
                //Se elimina el job de Hangfire
                BackgroundJob.Delete(jobIdPago);
                await _subastaJobRepository.EliminarJobIdAsync(subastaId, "Pago");
            }
        }

        /// <summary>
        /// Método que se encarga de publicar el evento SubastaIniciadaEvent para que lo consuma la máquina de estados.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        /// <param name="fechaInicio">Parámetro que corresponde a la fecha inicio de la subasta.</param>
        /// <param name="fechaFin">Parámetro que corresponde a la fecha fin de la subasta.</param>
        public async Task PublicarInicio(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            await _bus.Publish(new SubastaIniciadaEvent(subastaId, fechaInicio, fechaFin));
        }

        /// <summary>
        /// Método que se encarga de publicar el evento SubastaFinalizadaEvent para que lo consuma la máquina de estados.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        /// <param name="fechaInicio">Parámetro que corresponde a la fecha inicio de la subasta.</param>
        /// <param name="fechaFin">Parámetro que corresponde a la fecha fin de la subasta.</param>
        public async Task PublicarFin(Guid subastaId, DateTime fechaInicio, DateTime fechaFin)
        {
            await _bus.Publish(new SubastaFinalizadaEvent(subastaId, fechaInicio, fechaFin));
        }

        /// <summary>
        /// Método que se encarga de publicar el evento PublicarTemporizadorPago para que lo consuma la máquina de estados.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        public async Task PublicarTemporizadorPago(Guid subastaId)
        {
            await _bus.Publish(new TemporizadorCancelarPagoEvent(subastaId));
        }


    }
}
