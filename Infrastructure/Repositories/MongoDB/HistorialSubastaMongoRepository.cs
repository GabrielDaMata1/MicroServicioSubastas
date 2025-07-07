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
    /// Clase repository que implementa las operaciones que se pueden realizar sobre el historial de subastas almacenadas en MongoDB.
    /// </summary>
    public class HistorialSubastaMongoRepository: IHistorialSubastaMongoRepository
    {
        /// <summary>
        /// Atributo que corresponde a la colección de historial de subastas en la base de datos en MongoDB.
        /// </summary>
        private readonly IMongoCollection<HistorialSubastaMongo> _historialCollection;


        public HistorialSubastaMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("MicroservicioSubasta");
            _historialCollection = database.GetCollection<HistorialSubastaMongo>("HistorialSubasta");
        }

        /// <summary>
        /// Metodo que se encarga de registrar el historial de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="historialSubasta">Parámetro que corresponde a un objeto HistorialSubasta con su detalle.</param>
        /// <param name="resultado">Parámetro que corresponde al resultado final de la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        public async Task<HttpStatusCode> registrarHistorialSubastaAsync(HistorialSubasta historialSubasta, string resultado)
        {
            try
            {

                _historialCollection.InsertOneAsync(historialSubasta.ToMongo(resultado));
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar el historial de la subasta en MongoDB: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas por un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parámetro que corresponde al ID del usuario a consultar.</param>
        /// <returns>Retorna una lista de objetos HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<List<HistorialSubasta>> ObtenerSubastasGanadasPorUsuario(Guid idUsuario)
        {
            var filtro = Builders<HistorialSubastaMongo>.Filter.And(
                Builders<HistorialSubastaMongo>.Filter.Eq(s => s.IdUsuario, idUsuario),
                Builders<HistorialSubastaMongo>.Filter.Eq(s => s.Resultado, "Ganador") 
            );

            var subastasMongo = await _historialCollection.Find(filtro).ToListAsync();

            var subastas = subastasMongo.Select(s => HistorialSubastaFactory.CrearHistorialSubastaConID(s.Id, s.IdUsuario, s.IdSubasta, s.MontoFinal)).ToList(); 
            return subastas;
        }

        /// <summary>
        /// Metodo que se encarga de consultar las subastas ganadas en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Subasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<List<HistorialSubasta>> ObtenerSubastasGanadas()
        {
            var filtro = Builders<HistorialSubastaMongo>.Filter.And(
                Builders<HistorialSubastaMongo>.Filter.Eq(s => s.Resultado, "Ganador")
            );

            var subastasMongo = await _historialCollection.Find(filtro).ToListAsync();

            var subastas = subastasMongo.Select(s => HistorialSubastaFactory.CrearHistorialSubastaConID(s.Id, s.IdUsuario, s.IdSubasta, s.MontoFinal)).ToList();
            return subastas;
        }
        /// <summary>
        /// Metodo que se encarga de consultar el historial de una subasta en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a consultar.</param>
        /// <returns>Retorna un de objeto HistorialSubasta con su detalle si la operación ocurrió correctamente</returns>
        public async Task<HistorialSubasta> ObtenerHistorialSubasta(Guid idSubasta)
        {
            var subastasMongo = await _historialCollection.Find(r => r.IdSubasta == idSubasta).FirstOrDefaultAsync();

            var historial = HistorialSubastaFactory.CrearHistorialSubastaConID(subastasMongo.Id, subastasMongo.IdUsuario, subastasMongo.IdSubasta, subastasMongo.MontoFinal);
            return historial;
        }
    }
}
