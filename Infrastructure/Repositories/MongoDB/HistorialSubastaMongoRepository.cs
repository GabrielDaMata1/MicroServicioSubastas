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

namespace Infrastructure.Repositories.MongoDB
{
    public class HistorialSubastaMongoRepository: IHistorialSubastaMongoRepository

    {
    private readonly IMongoCollection<HistorialSubastaMongo> _historialCollection;


    public HistorialSubastaMongoRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("MicroservicioSubasta");
        _historialCollection = database.GetCollection<HistorialSubastaMongo>("HistorialSubasta");
    }

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

    }
}
