using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Models.MongoDB;
using MongoDB.Driver;

namespace Infrastructure.Repositories.MongoDB
{
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre las subastas almacenadas en MongoDB.
    /// </summary>
    public class SubastaMongoRepository : ISubastaRepositoryMongo
    {
        /// <summary>
        /// Atributo que corresponde a la colección de subastas en la base de datos en MongoDB.
        /// </summary>
        private readonly IMongoCollection<SubastaMongo> _subastaCollection;
        /// <summary>
        /// Atributo que corresponde al repositorio del historial de subastas en la base de datos en MongoDB.
        /// </summary>
        private readonly IHistorialSubastaMongoRepository _historialSubastaMongoRepository;



        public SubastaMongoRepository(IMongoClient mongoClient, IHistorialSubastaMongoRepository historialSubastaMongoRepository)
        {
            var database = mongoClient.GetDatabase("MicroservicioSubasta");
            _subastaCollection = database.GetCollection<SubastaMongo>("Subasta");
            _historialSubastaMongoRepository = historialSubastaMongoRepository;
        }

        /// <summary>
        /// Metodo que se encarga de registrar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que registra la subasta.</param>
        /// <returns>Retorna el GUID correspondiente a la subasta dada</returns>
        public async Task<HttpStatusCode> RegistrarSubastaAsync(Subasta subasta, Guid IdUsuario)
        {
            try
            {

                _subastaCollection.InsertOneAsync(subasta.ToMongo(IdUsuario));
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar el producto en MongoDB: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de modificar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que modifica la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        public async Task<HttpStatusCode> ModificarSubastaAsync(Subasta subasta,  Guid IdUsuario)
        {
            var resultado = await _subastaCollection.ReplaceOneAsync(
                u => u.Id == subasta.Id, subasta.ToMongo(IdUsuario));
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Metodo que se encarga de consultar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a consultar.</param>
        /// <returns>Retorna un objeto Subasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<Subasta> ObtenerSubastaPorId(Guid idSubasta)
        {
            var subastaMongo = await _subastaCollection.Find(r => r.Id == idSubasta).FirstOrDefaultAsync();
            var subastaEntidad = SubastaFactory.CrearSubastaConId(subastaMongo.Id, subastaMongo.Nombre,
                subastaMongo.Descripcion, subastaMongo.ProductoId, subastaMongo.FechaInicio, subastaMongo.FechaFin,
                subastaMongo.IncrementoMinimo, subastaMongo.PrecioReserva, subastaMongo.Estado);

            return subastaEntidad;
        }

        /// <summary>
        /// Metodo que se encarga de eliminar una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano si la operación ocurrió correctamente</returns>
        public async Task<bool> EliminarSubastaAsync(Guid idSubasta)
        {
            var filtro = Builders<SubastaMongo>.Filter.Eq(p => p.Id, idSubasta);
            var resultado = await _subastaCollection.DeleteOneAsync(filtro);

            return resultado.DeletedCount > 0;
        }

        /// <summary>
        /// Metodo que se encarga de consultar el ID de un subastador que organizó la subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a consultar.</param>
        /// <returns>Retorna un GUID que le corresponde al subastador si la operación ocurrió correctamente</returns>
        public async Task<Guid> ObtenerUsuarioIdPorSubastaId(Guid idSubasta)
        {
            var subastaMongo = await _subastaCollection.Find(r => r.Id == idSubasta).FirstOrDefaultAsync();

            return subastaMongo.IdUsuario;
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<List<Subasta>> ObtenerSubastas()
        {
            var subastasMongo = await _subastaCollection.Find(_ => true).ToListAsync();

            var subastas = subastasMongo.Select(s => SubastaFactory.CrearSubastaConIdUsuario(s.Id, s.Nombre, s.Descripcion,
                s.ProductoId, s.FechaInicio, s.FechaFin, s.IncrementoMinimo, s.PrecioReserva, s.Estado, s.IdUsuario)).ToList();

            return subastas;
        }

        /// <summary>
        /// Metodo que se encarga de consultar una subasta específica en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna un objeto Subasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<Subasta> ObtenerSubasta(Guid idSubasta)
        {
            var subastaMongo = await _subastaCollection.Find(r => r.Id == idSubasta).FirstOrDefaultAsync();
            var subasta = SubastaFactory.CrearSubastaConIdUsuario(subastaMongo.Id, subastaMongo.Nombre, subastaMongo.Descripcion,
                subastaMongo.ProductoId, subastaMongo.FechaInicio, subastaMongo.FechaFin, subastaMongo.IncrementoMinimo, subastaMongo.PrecioReserva, subastaMongo.Estado, subastaMongo.IdUsuario);

            return subasta;
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas organizadas por un subastador en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del subastador a consultar.</param>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<List<Subasta>> ObtenerSubastasPorUsuario(Guid idUsuario)
        {
            var filtro = Builders<SubastaMongo>.Filter.Eq(s => s.IdUsuario, idUsuario);
            var subastasMongo = await _subastaCollection.Find(filtro).ToListAsync();

            var subastas = subastasMongo.Select(s => SubastaFactory.CrearSubastaConId(s.Id, s.Nombre, s.Descripcion, s.ProductoId, s.FechaInicio, s.FechaFin,
                s.IncrementoMinimo, s.PrecioReserva, s.Estado)).ToList();

            return subastas;
        }

        /// <summary>
        /// Metodo que se encarga de modificar el estado de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a modificar.</param>
        /// <param name="nuevoEstado">Parámetro que corresponde al nuevo estado de la subasta a asignar.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        public async Task<HttpStatusCode> ActualizarEstadoSubasta(Guid idSubasta, string nuevoEstado)
        {
            var filtro = Builders<SubastaMongo>.Filter.Eq(s => s.Id, idSubasta);
            var actualizacion = Builders<SubastaMongo>.Update.Set(s => s.Estado, nuevoEstado);

            await _subastaCollection.UpdateOneAsync(filtro, actualizacion);
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<List<Subasta>> ObtenerSubastasGanadasDetalle(Guid idUsuario)
        {
            var historial = await _historialSubastaMongoRepository.ObtenerSubastasGanadasPorUsuario(idUsuario);
            if (historial == null || !historial.Any())
                return new List<Subasta>();

            var idsSubasta = historial.Select(h => h.IdSubasta).Distinct().ToList();

            var filtro = Builders<SubastaMongo>.Filter.In(s => s.Id, idsSubasta);
            var subastasMongo = await _subastaCollection.Find(filtro).ToListAsync();


            var subastas = subastasMongo.Select(s =>SubastaFactory.CrearSubastaConId(s.Id,s.Nombre,s.Descripcion,s.ProductoId, s.FechaInicio, s.FechaFin, s.IncrementoMinimo,
                    s.PrecioReserva,s.Estado)).ToList();


            return subastas;
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario con su detalle en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<List<Subasta>> ObtenerSubastasGanadas()
        {
            var historial = await _historialSubastaMongoRepository.ObtenerSubastasGanadas();
            if (historial == null || !historial.Any())
                return new List<Subasta>();

            var idsSubasta = historial.Select(h => h.IdSubasta).Distinct().ToList();

            var filtro = Builders<SubastaMongo>.Filter.In(s => s.Id, idsSubasta);
            var subastasMongo = await _subastaCollection.Find(filtro).ToListAsync();


            var subastas = subastasMongo.Select(s => SubastaFactory.CrearSubastaConIdUsuario(s.Id, s.Nombre, s.Descripcion, s.ProductoId, s.FechaInicio, s.FechaFin, s.IncrementoMinimo,
                    s.PrecioReserva, s.Estado, s.IdUsuario)).ToList();
            return subastas;
        }
    }
}
