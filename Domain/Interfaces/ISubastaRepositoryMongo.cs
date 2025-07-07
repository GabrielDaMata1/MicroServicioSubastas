using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MongoDB.Driver;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre las subastas almacenadas en MongoDB.
    /// </summary>
    public interface ISubastaRepositoryMongo
    {
        /// <summary>
        /// Metodo que se encarga de registrar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que registra la subasta.</param>
        /// <returns>Retorna el GUID correspondiente a la subasta dada</returns>
        Task<HttpStatusCode> RegistrarSubastaAsync(Subasta subasta, Guid IdUsuario);
        /// <summary>
        /// Metodo que se encarga de modificar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que modifica la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> ModificarSubastaAsync(Subasta subasta, Guid IdUsuario);
        /// <summary>
        /// Metodo que se encarga de consultar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a consultar.</param>
        /// <returns>Retorna un objeto Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<Subasta> ObtenerSubastaPorId(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de eliminar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano si la operación ocurrió correctamente</returns>
        Task<bool> EliminarSubastaAsync(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de consultar el ID de un subastador que organizó la subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a consultar.</param>
        /// <returns>Retorna un GUID que le corresponde al subastador si la operación ocurrió correctamente</returns>
        Task<Guid> ObtenerUsuarioIdPorSubastaId(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<Subasta>> ObtenerSubastas();
        /// <summary>
        /// Metodo que se encarga de consultar una subasta específica en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna un objeto Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<Subasta> ObtenerSubasta(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas organizadas por un subastador en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del subastador a consultar.</param>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<Subasta>> ObtenerSubastasPorUsuario(Guid idUsuario);
        /// <summary>
        /// Metodo que se encarga de modificar el estado de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a modificar.</param>
        /// <param name="nuevoEstado">Parámetro que corresponde al nuevo estado de la subasta a asignar.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> ActualizarEstadoSubasta(Guid idSubasta, string nuevoEstado);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<Subasta>> ObtenerSubastasGanadasDetalle(Guid idUsuario);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario con su detalle en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<Subasta>> ObtenerSubastasGanadas();


    }
}
