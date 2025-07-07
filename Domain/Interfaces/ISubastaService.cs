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
    /// Clase interface que define las operaciones que se pueden realizar sobre las subastas almacenadas en ambas bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public interface ISubastaService
    {
        /// <summary>
        /// Metodo que se encarga de registrar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que registra la subasta.</param>
        Task<Guid> RegistrarSubastaPostgreSQLAsync(Subasta subasta, Guid IdUsuario);
        /// <summary>
        /// Metodo que se encarga de registrar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que registra la subasta.</param>
        /// <returns>Retorna el GUID correspondiente a la subasta dada</returns>
        Task<HttpStatusCode> RegistrarSubastaMongoAsync(Subasta subasta, Guid IdUsuario);
        /// <summary>
        /// Metodo que se encarga de modificar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que modifica la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> ModificarSubastaPostgreSQLAsync(Subasta subasta, Guid IdUsuario);
        /// <summary>
        /// Metodo que se encarga de modificar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que modifica la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> ModificarSubastaMongoAsync(Subasta subasta, Guid IdUsuario);
        /// <summary>
        /// Metodo que se encarga de consultar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a consultar.</param>
        /// <returns>Retorna un objeto Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<Subasta> ObtenerSubastaPorIdMongoAsync(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de eliminar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano si la operación ocurrió correctamente</returns>
        Task<bool> EliminarSubastaMongoAsync(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de eliminar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano si la operación ocurrió correctamente</returns>
        Task<bool> EliminarSubastaPostgreSQLAsync(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de consultar el ID de un subastador que organizó la subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a consultar.</param>
        /// <returns>Retorna un GUID que le corresponde al subastador si la operación ocurrió correctamente</returns>
        Task<Guid> ObtenerUsuarioIdPorSubastaIdMongoAsync(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<Subasta>> ObtenerSubastasMongo();
        /// <summary>
        /// Metodo que se encarga de consultar una subasta específica en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna un objeto Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<Subasta> ObtenerSubastaMongoAsync(Guid idSubasta);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas organizadas por un subastador en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del subastador a consultar.</param>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<Subasta>> ObtenerSubastasPorUsuarioMongoAsync(Guid idUsuario);
        /// <summary>
        /// Metodo que se encarga de modificar el estado de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a modificar.</param>
        /// <param name="nuevoEstado">Parámetro que corresponde al nuevo estado de la subasta a asignar.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> ActualizarEstadoSubastaMongoAsync(Guid idSubasta, string nuevoEstado);
        /// <summary>
        /// Metodo que se encarga de modificar el estado de una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a modificar.</param>
        /// <param name="nuevoEstado">Parámetro que corresponde al nuevo estado de la subasta a asignar.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> ActualizarEstadoSubastaPostgreSQLAsync(Guid idSubasta, string nuevoEstado);
        /// <summary>
        /// Metodo que se encarga de registrar el historial de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="historialSubasta">Parámetro que corresponde a un objeto HistorialSubasta con su detalle.</param>
        /// <param name="resultado">Parámetro que corresponde al resultado final de la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<HttpStatusCode> RegistrarHistorialSubastaMongoAsync(HistorialSubasta historialSubasta, string resultado);
        /// <summary>
        /// Metodo que se encarga de registrar el historial de una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="historialSubasta">Parámetro que corresponde a un objeto HistorialSubasta con su detalle.</param>
        /// <param name="resultado">Parámetro que corresponde al resultado final de la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        Task<Guid> RegistrarHistorialSubastaPostgreSQLAsync(HistorialSubasta historialSubasta, string resultado);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<HistorialSubasta>> ObtenerSubastasGanadasPorUsuarioMongoAsync(Guid idUsuario);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas con su detalle por un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<Subasta>> ObtenerSubastasGanadasDetalleMongoAsync(Guid idUsuario);
        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        Task<List<Subasta>> ObtenerSubastasGanadasMongoAsync();
        /// <summary>
        /// Metodo que se encarga de consultar el historial de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a consultar.</param>
        /// <returns>Retorna un de objeto HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        Task<HistorialSubasta> ObtenerHistorialSubastaMongoAsync(Guid idSubasta);



    }
}
