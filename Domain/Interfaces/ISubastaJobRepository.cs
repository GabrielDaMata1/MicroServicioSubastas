using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre los jobs de hangfire.
    /// </summary>
    public interface ISubastaJobRepository
    {
        /// <summary>
        /// Método que se encarga de registrar los datos de un job en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subastaId">Parametro que corresponde al ID de la subasta</param>
        /// <param name="jobType">Parametro que corresponde al tipo de Job (Inicio, Fin, Pago)</param>
        /// <param name="hangfireJobId">Parametro que corresponde al ID del job en Hangfire</param>
        Task GuardarJobAsync(Guid subastaId, string jobType, string hangfireJobId);
        /// <summary>
        /// Método que se encarga de obtener el ID de un job en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subastaId">Parametro que corresponde al ID de la subasta</param>
        /// <param name="jobType">Parametro que corresponde al tipo de Job (Inicio, Fin, Pago)</param>
        /// <returns>Retorna un string con el valor del ID del job en hangfire</returns>
        Task<string> ObtenerJobIdAsync(Guid subastaId, string jobType);
        /// <summary>
        /// Método que se encarga de eliminar un job en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subastaId">Parametro que corresponde al ID de la subasta</param>
        /// <param name="jobType">Parametro que corresponde al tipo de Job (Inicio, Fin, Pago)</param>
        Task EliminarJobIdAsync(Guid subastaId, string jobType);
    }
}
