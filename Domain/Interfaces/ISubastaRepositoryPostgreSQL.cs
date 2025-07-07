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
    /// Clase interface que define las operaciones que se pueden realizar sobre las subastas almacenadas en PostgreSQL.
    /// </summary>
    public interface ISubastaRepositoryPostgreSQL
    {
        /// <summary>
        /// Metodo que se encarga de registrar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que registra la subasta.</param>
        Task<Guid> RegistrarSubastaAsync(Subasta subasta, Guid IdUsuario);
        /// <summary>
        /// Metodo que se encarga de modificar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que modifica la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> ModificarSubastaAsync(Subasta subasta, Guid IdUsuario);
        /// <summary>
        /// Metodo que se encarga de eliminar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano si la operación ocurrió correctamente</returns>
        Task<bool> EliminarSubastaAsync(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de modificar el estado de una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a modificar.</param>
        /// <param name="nuevoEstado">Parámetro que corresponde al nuevo estado de la subasta a asignar.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> ActualizarEstadoSubasta(Guid idSubasta, string nuevoEstado);
    }
}
