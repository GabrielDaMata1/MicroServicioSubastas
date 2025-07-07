using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre las jobs en Hangfire.
    /// </summary>
    public interface ISubastaSchedule
    {

        /// <summary>
        /// Método que se encarga de programar los eventos de inicio, fin y temporizador de pago para una subasta.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        /// <param name="fechaInicio">Parámetro que corresponde a la fecha inicio de la subasta.</param>
        /// <param name="fechaFin">Parámetro que corresponde a la fecha fin de la subasta.</param>
        Task ProgramarEventosDeSubasta(Guid subastaId, DateTime fechaInicio, DateTime fechaFin);
        /// <summary>
        /// Método que se encarga de publicar el evento SubastaIniciadaEvent para que lo consuma la máquina de estados.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        /// <param name="fechaInicio">Parámetro que corresponde a la fecha inicio de la subasta.</param>
        /// <param name="fechaFin">Parámetro que corresponde a la fecha fin de la subasta.</param>
        Task PublicarInicio(Guid subastaId, DateTime fechaInicio, DateTime fechaFin);
        /// <summary>
        /// Método que se encarga de publicar el evento SubastaFinalizadaEvent para que lo consuma la máquina de estados.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        /// <param name="fechaInicio">Parámetro que corresponde a la fecha inicio de la subasta.</param>
        /// <param name="fechaFin">Parámetro que corresponde a la fecha fin de la subasta.</param>
        Task PublicarFin(Guid subastaId, DateTime fechaInicio, DateTime fechaFin);
        /// <summary>
        /// Método que se encarga de reprogramar los eventos de inicio, fin y temporizador de pago para una subasta.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        /// <param name="nuevaFechaInicio">Parámetro que corresponde a la nueva fecha inicio de la subasta.</param>
        /// <param name="nuevaFechaFin">Parámetro que corresponde a la nueva fecha fin de la subasta.</param>
        Task ReprogramarEventosDeSubasta(Guid subastaId, DateTime nuevaFechaInicio, DateTime nuevaFechaFin);
        /// <summary>
        /// Método que se encarga de eliminar el temporizador de cancelación de una subasta por la falta de pago.
        /// </summary>
        /// <param name="subastaId">Parámetro que corresponde al ID de la subasta.</param>
        Task EliminarTemporizadorPagoSubasta(Guid subastaId);
    }
}
