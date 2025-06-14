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
    }
}
