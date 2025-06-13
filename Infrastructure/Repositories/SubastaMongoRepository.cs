using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
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

        public async Task<HttpStatusCode> RegistrarProductoAsync(Subasta subasta, Guid IdUsuario)
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

    }
}
