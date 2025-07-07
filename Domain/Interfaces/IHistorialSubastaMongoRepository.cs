using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre el historial de subastas almacenado en MongoDB.
    /// </summary>
    public interface IHistorialSubastaMongoRepository
    {
        /// <summary>
        /// Metodo que se encarga de registrar el historial de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="historialSubasta">Parámetro que corresponde a un objeto HistorialSubasta con su detalle.</param>
        /// <param name="resultado">Parámetro que corresponde al resultado final de la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> registrarHistorialSubastaAsync(HistorialSubasta historialSubasta, string resultado);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<HistorialSubasta>> ObtenerSubastasGanadasPorUsuario(Guid idUsuario);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<HistorialSubasta>> ObtenerSubastasGanadas();
        /// <summary>
        /// Metodo que se encarga de consultar el historial de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a consultar.</param>
        /// <returns>Retorna un de objeto HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        Task<HistorialSubasta> ObtenerHistorialSubasta(Guid idSubasta);
    }
}
