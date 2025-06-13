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
    public class SubastaService : ISubastaService

    {
        private readonly ISubastaRepositoryMongo _subastaMongoRepository;
        private readonly ISubastaRepositoryPostgreSQL _subastaPostgreSQLRepository;


        public SubastaService(ISubastaRepositoryMongo subastaMongoRepository, ISubastaRepositoryPostgreSQL subastaPostgreSQLRepository)

        {
            _subastaMongoRepository = subastaMongoRepository;
            _subastaPostgreSQLRepository = subastaPostgreSQLRepository;

        }

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

        public async Task<HttpStatusCode> RegistrarProductoMongoAsync(Subasta subasta, Guid IdUsuario)
        {
            try
            {
                var resul = await _subastaMongoRepository.RegistrarProductoAsync(subasta, IdUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar la subasta en MongoDB {ex.Message}", ex);
            }
        }
    }
}
