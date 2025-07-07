using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre el historial de subastas almacenado en PostgreSQL.
    /// </summary>
    public interface IHistorialSubastaPostgreSQLRepository
    {
        /// <summary>
        /// Metodo que se encarga de registrar el historial de una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="historialSubasta">Parámetro que corresponde a un objeto HistorialSubasta con su detalle.</param>
        /// <param name="resultado">Parámetro que corresponde al resultado final de la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<Guid> registrarHistorialSubastaAsync(HistorialSubasta historialSubasta, string resultado);
    }
}
