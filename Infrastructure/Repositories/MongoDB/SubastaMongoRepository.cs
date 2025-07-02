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
    public class SubastaMongoRepository : ISubastaRepositoryMongo
    {
        private readonly IMongoCollection<SubastaMongo> _subastaCollection;
        private readonly IHistorialSubastaMongoRepository _historialSubastaMongoRepository;



        public SubastaMongoRepository(IMongoClient mongoClient, IHistorialSubastaMongoRepository historialSubastaMongoRepository)
        {
            var database = mongoClient.GetDatabase("MicroservicioSubasta");
            _subastaCollection = database.GetCollection<SubastaMongo>("Subasta");
            _historialSubastaMongoRepository = historialSubastaMongoRepository;
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
                subastaMongo.IncrementoMinimo, subastaMongo.PrecioReserva, subastaMongo.Estado);

            return subastaEntidad;
        }

        public async Task<bool> EliminarSubastaAsync(Guid idSubasta)
        {
            var filtro = Builders<SubastaMongo>.Filter.Eq(p => p.Id, idSubasta);
            var resultado = await _subastaCollection.DeleteOneAsync(filtro);

            return resultado.DeletedCount > 0;
        }
        public async Task<Guid> ObtenerUsuarioIdPorSubastaId(Guid idSubasta)
        {
            var subastaMongo = await _subastaCollection.Find(r => r.Id == idSubasta).FirstOrDefaultAsync();

            return subastaMongo.IdUsuario;
        }



        public async Task<List<Subasta>> ObtenerSubastas()
        {
            var subastasMongo = await _subastaCollection.Find(_ => true).ToListAsync();

            var subastas = subastasMongo.Select(s => SubastaFactory.CrearSubastaConId(s.Id, s.Nombre, s.Descripcion,
                s.ProductoId, s.FechaInicio, s.FechaFin, s.IncrementoMinimo, s.PrecioReserva, s.Estado)).ToList();

            return subastas;
        }

        public async Task<Subasta> ObtenerSubasta(Guid idSubasta)
        {
            var subastaMongo = await _subastaCollection.Find(r => r.Id == idSubasta).FirstOrDefaultAsync();
            var subasta = SubastaFactory.CrearSubastaConId(subastaMongo.Id, subastaMongo.Nombre, subastaMongo.Descripcion,
                subastaMongo.ProductoId, subastaMongo.FechaInicio, subastaMongo.FechaFin, subastaMongo.IncrementoMinimo, subastaMongo.PrecioReserva, subastaMongo.Estado);

            return subasta;
        }

        public async Task<List<Subasta>> ObtenerSubastasPorUsuario(Guid idUsuario)
        {
            var filtro = Builders<SubastaMongo>.Filter.Eq(s => s.IdUsuario, idUsuario);
            var subastasMongo = await _subastaCollection.Find(filtro).ToListAsync();

            var subastas = subastasMongo.Select(s => SubastaFactory.CrearSubastaConId(s.Id, s.Nombre, s.Descripcion, s.ProductoId, s.FechaInicio, s.FechaFin,
                s.IncrementoMinimo, s.PrecioReserva, s.Estado)).ToList();

            return subastas;
        }

        public async Task<HttpStatusCode> ActualizarEstadoSubasta(Guid idSubasta, string nuevoEstado)
        {
            var filtro = Builders<SubastaMongo>.Filter.Eq(s => s.Id, idSubasta);
            var actualizacion = Builders<SubastaMongo>.Update.Set(s => s.Estado, nuevoEstado);

            await _subastaCollection.UpdateOneAsync(filtro, actualizacion);
            return HttpStatusCode.OK;
        }

        public async Task<List<Subasta>> ObtenerSubastasGanadasDetalle(Guid idUsuario)
        {
            var historial = await _historialSubastaMongoRepository.ObtenerSubastasGanadasPorUsuario(idUsuario);
            if (historial == null || !historial.Any())
                return new List<Subasta>();

            Console.WriteLine(historial.ElementAt(0).IdUsuario);
            Console.WriteLine(historial.ElementAt(0).IdSubasta);

            var idsSubasta = historial.Select(h => h.IdSubasta).Distinct().ToList();

            Console.WriteLine(idsSubasta);
            Console.WriteLine(idsSubasta.ElementAt(0));

            var idDePrueba = idsSubasta.First();
            var existe = await _subastaCollection.Find(s => s.Id == idDePrueba).AnyAsync();
            Console.WriteLine($"¿Existe subasta con ID {idDePrueba}? {existe}");

            var filtro = Builders<SubastaMongo>.Filter.In(s => s.Id, idsSubasta);
            var subastasMongo = await _subastaCollection.Find(filtro).ToListAsync();


            var subastas = subastasMongo.Select(s =>SubastaFactory.CrearSubastaConId(s.Id,s.Nombre,s.Descripcion,s.ProductoId, s.FechaInicio, s.FechaFin, s.IncrementoMinimo,
                    s.PrecioReserva,s.Estado)).ToList();


            return subastas;
        }

    }
}
