using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Service
{
    /// <summary>
    /// Clase Service que se encarga de procesar todas las operaciones sobre una subasta, incluyendo las operaciones con bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public class SubastaService : ISubastaService
    {
        /// <summary>
        /// Atributo que corresponde al repositorio de subastas en la base de datos en MongoDB.
        /// </summary>
        private readonly ISubastaRepositoryMongo _subastaMongoRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio de subastas en la base de datos en PostgreSQL.
        /// </summary>
        private readonly ISubastaRepositoryPostgreSQL _subastaPostgreSQLRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio del historial de subastas en la base de datos en MongoDB.
        /// </summary>
        private readonly IHistorialSubastaPostgreSQLRepository _historialSubastaPostgreSQLRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio del historial de subastas en la base de datos en PostgreSQL.
        /// </summary>
        private readonly IHistorialSubastaMongoRepository _historialSubastaMongoRepository;


        public SubastaService(ISubastaRepositoryMongo subastaMongoRepository, ISubastaRepositoryPostgreSQL subastaPostgreSQLRepository, IHistorialSubastaMongoRepository historialSubastaMongoRepository, IHistorialSubastaPostgreSQLRepository historialSubastaPostgreSqlRepository)

        {
            _subastaMongoRepository = subastaMongoRepository;
            _subastaPostgreSQLRepository = subastaPostgreSQLRepository;
            _historialSubastaMongoRepository= historialSubastaMongoRepository;
            _historialSubastaPostgreSQLRepository = historialSubastaPostgreSqlRepository;

        }
        /// <summary>
        /// Metodo que se encarga de registrar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que registra la subasta.</param>
        /// <returns>Retorna el GUID correspondiente a la subasta dada</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar la subasta en la base de datos en PostgreSQL. 
        /// </exception>
        public async Task<Guid> RegistrarSubastaPostgreSQLAsync(Subasta subasta, Guid IdUsuario)
        {
            try
            {
                var resul = await _subastaPostgreSQLRepository.RegistrarSubastaAsync(subasta, IdUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar registrar la subasta en PostgreSQL {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de registrar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que registra la subasta.</param>
        /// <returns>Retorna el GUID correspondiente a la subasta dada</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar la subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<HttpStatusCode> RegistrarSubastaMongoAsync(Subasta subasta, Guid IdUsuario)
        {
            try
            {
                var resul = await _subastaMongoRepository.RegistrarSubastaAsync(subasta, IdUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar la subasta en MongoDB {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de modificar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que modifica la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al modificar la subasta en la base de datos en PostgreSQL. 
        /// </exception>
        public async Task<HttpStatusCode> ModificarSubastaPostgreSQLAsync(Subasta subasta, Guid IdUsuario)
        {
            try
            {
                var resul = await _subastaPostgreSQLRepository.ModificarSubastaAsync(subasta, IdUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar modificar la subasta en PostgreSQL {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de modificar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que modifica la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al modificar la subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<HttpStatusCode> ModificarSubastaMongoAsync(Subasta subasta, Guid IdUsuario)
        {
            try
            {
                var resul = await _subastaMongoRepository.ModificarSubastaAsync(subasta, IdUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar modificar la subasta en MongoDB {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de consultar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a consultar.</param>
        /// <returns>Retorna un objeto Subasta con su detalle si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar la subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<Subasta> ObtenerSubastaPorIdMongoAsync(Guid idSubasta)
        {
            try
            {
                var resul = await _subastaMongoRepository.ObtenerSubastaPorId(idSubasta);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener la subasta en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de eliminar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al eliminar la subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<bool> EliminarSubastaMongoAsync(Guid idSubasta)
        {
            try
            {
                var resul = await _subastaMongoRepository.EliminarSubastaAsync(idSubasta);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar eliminar la subasta en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de eliminar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano si la operación ocurrió correctamente</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al eliminar la subasta en la base de datos en PostgreSQL. 
        /// </exception>
        public async Task<bool> EliminarSubastaPostgreSQLAsync(Guid idSubasta)
        {
            try
            {
                var resul = await _subastaPostgreSQLRepository.EliminarSubastaAsync(idSubasta);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar eliminar la subasta en PostgreSQL {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de consultar el ID de un subastador que organizó la subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a consultar.</param>
        /// <returns>Retorna un GUID que le corresponde al subastador si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar la subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<Guid> ObtenerUsuarioIdPorSubastaIdMongoAsync(Guid idSubasta)
        {
            try
            {
                var resul = await _subastaMongoRepository.ObtenerUsuarioIdPorSubastaId(idSubasta);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener el id del usuario de la subasta en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar la subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<List<Subasta>> ObtenerSubastasMongo()
        {
            try
            {
                var resul = await _subastaMongoRepository.ObtenerSubastas();
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener las subastas en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de consultar una subasta específica en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna un objeto Subasta con su detalle si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar la subasta en la base de datos en MongoDB. 
        /// </exception>

        public async Task<Subasta> ObtenerSubastaMongoAsync(Guid idSubasta)
        {
            try
            {
                var resul = await _subastaMongoRepository.ObtenerSubasta(idSubasta);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener la subasta en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas organizadas por un subastador en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del subastador a consultar.</param>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar la subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<List<Subasta>> ObtenerSubastasPorUsuarioMongoAsync(Guid idUsuario)
        {
            try
            {
                var resul = await _subastaMongoRepository.ObtenerSubastasPorUsuario(idUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener las subastas en MongoDB {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Metodo que se encarga de modificar el estado de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a modificar.</param>
        /// <param name="nuevoEstado">Parámetro que corresponde al nuevo estado de la subasta a asignar.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al modificar la subasta en la base de datos en MongoDB. 
        /// </exception>

        public async Task<HttpStatusCode> ActualizarEstadoSubastaMongoAsync(Guid idSubasta, string nuevoEstado)
        {
            try
            {
                var resul = await _subastaMongoRepository.ActualizarEstadoSubasta(idSubasta, nuevoEstado);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar modificar el estado de la subasta en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de modificar el estado de una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a modificar.</param>
        /// <param name="nuevoEstado">Parámetro que corresponde al nuevo estado de la subasta a asignar.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al modificar la subasta en la base de datos en PostgreSQL. 
        /// </exception>
        public async Task<HttpStatusCode> ActualizarEstadoSubastaPostgreSQLAsync(Guid idSubasta, string nuevoEstado)
        {
            try
            {
                var resul = await _subastaPostgreSQLRepository.ActualizarEstadoSubasta(idSubasta, nuevoEstado);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar modificar el estado de la subasta en PostgreSQL {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de registrar el historial de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="historialSubasta">Parámetro que corresponde a un objeto HistorialSubasta con su detalle.</param>
        /// <param name="resultado">Parámetro que corresponde al resultado final de la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el historial de una subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<HttpStatusCode> RegistrarHistorialSubastaMongoAsync(HistorialSubasta historialSubasta, string resultado)
        {
            try
            {
                var resul = await _historialSubastaMongoRepository.registrarHistorialSubastaAsync(historialSubasta,resultado);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar el historial de la subasta en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de registrar el historial de una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="historialSubasta">Parámetro que corresponde a un objeto HistorialSubasta con su detalle.</param>
        /// <param name="resultado">Parámetro que corresponde al resultado final de la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el historial de una subasta en la base de datos en PostgreSQL. 
        /// </exception>
        public async Task<Guid> RegistrarHistorialSubastaPostgreSQLAsync(HistorialSubasta historialSubasta, string resultado)
        {
            try
            {
                var resul = await _historialSubastaPostgreSQLRepository.registrarHistorialSubastaAsync(historialSubasta,resultado);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar registrar el historial de la subasta en PostgreSQL {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar el historial de subastas en la base de datos en MongoDB. 
        /// </exception>
        public async Task<List<HistorialSubasta>> ObtenerSubastasGanadasPorUsuarioMongoAsync(Guid idUsuario)
        {

            try
            {
                var resul = await _historialSubastaMongoRepository.ObtenerSubastasGanadasPorUsuario(idUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener el historial de la subasta en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario con su detalle en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar el historial de subastas en la base de datos en MongoDB. 
        /// </exception>

        public async Task<List<Subasta>> ObtenerSubastasGanadasDetalleMongoAsync(Guid idUsuario)
        {
            try
            {
                var resul = await _subastaMongoRepository.ObtenerSubastasGanadasDetalle(idUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener las subastas detalladas ganadas en MongoDB {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar la subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<List<Subasta>> ObtenerSubastasGanadasMongoAsync()
        {
            try
            {
                var resul = await _subastaMongoRepository.ObtenerSubastasGanadas();
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener las subastas ganadas en MongoDB {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de consultar el historial de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a consultar.</param>
        /// <returns>Retorna un de objeto HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar el historial de subasta en la base de datos en MongoDB. 
        /// </exception>
        public async Task<HistorialSubasta> ObtenerHistorialSubastaMongoAsync(Guid idSubasta)
        {
            try
            {
                var resul = await _historialSubastaMongoRepository.ObtenerHistorialSubasta(idSubasta);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener el historial de la subasta en MongoDB {ex.Message}", ex);
            }
        }
    }
}
