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

namespace Infrastructure.Repositories
{
    public class SubastaMongoRepository : ISubastaRepositoryMongo
    {
        private readonly IMongoCollection<SubastaMongo> _subastaCollection;


        public SubastaMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("MicroservicioSubasta");
            _subastaCollection = database.GetCollection<SubastaMongo>("Subasta");
        }

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

        public async Task<HttpStatusCode> ModificarSubastaAsync(Subasta subasta,  Guid IdUsuario)
        {
            var resultado = await _subastaCollection.ReplaceOneAsync(
                u => u.Id == subasta.Id, subasta.ToMongo(IdUsuario));
            return HttpStatusCode.OK;
        }

        public async Task<Subasta> ObtenerSubastaPorId(Guid idSubasta)
        {
            var subastaMongo = await _subastaCollection.Find(r => r.Id == idSubasta).FirstOrDefaultAsync();
            var subastaEntidad = SubastaFactory.CrearSubastaConId(subastaMongo.Id, subastaMongo.Nombre,
                subastaMongo.Descripcion, subastaMongo.ProductoId, subastaMongo.FechaInicio, subastaMongo.FechaFin,
                subastaMongo.IncrementoMinimo, subastaMongo.PrecioReserva);

            return subastaEntidad;
        }
    }
}
