using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Models.MongoDB;
using MongoDB.Driver;

namespace Infrastructure.Repositories.MongoDB
{
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre los jobs de subastas almacenadas en Hangfire.
    /// </summary>
    public class SubastaJobRepository : ISubastaJobRepository
    {
        /// <summary>
        /// Atributo que corresponde a la colección de jobs de subastas en la base de datos en MongoDB.
        /// </summary>
        private readonly IMongoCollection<SubastaJobMongo> _jobCollection;



        public SubastaJobRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("HangFireBD");
            _jobCollection = database.GetCollection<SubastaJobMongo>("SubastaJobs");
        }
        /// <summary>
        /// Método que se encarga de registrar los datos de un job en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subastaId">Parametro que corresponde al ID de la subasta</param>
        /// <param name="jobType">Parametro que corresponde al tipo de Job (Inicio, Fin, Pago)</param>
        /// <param name="hangfireJobId">Parametro que corresponde al ID del job en Hangfire</param>
        public async Task GuardarJobAsync(Guid subastaId, string jobType, string hangfireJobId)
        {
            var filter = Builders<SubastaJobMongo>.Filter.Where(
                e => e.SubastaId == subastaId && e.JobType == jobType);

            var update = Builders<SubastaJobMongo>.Update
                .Set(e => e.HangfireJobId, hangfireJobId);

            var options = new UpdateOptions { IsUpsert = true };

            await _jobCollection.UpdateOneAsync(filter, update, options);
        }
        /// <summary>
        /// Método que se encarga de obtener el ID de un job en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subastaId">Parametro que corresponde al ID de la subasta</param>
        /// <param name="jobType">Parametro que corresponde al tipo de Job (Inicio, Fin, Pago)</param>
        /// <returns>Retorna un string con el valor del ID del job en hangfire</returns>
        public async Task<string> ObtenerJobIdAsync(Guid subastaId, string jobType)
        {
            var filter = Builders<SubastaJobMongo>.Filter.Where(
                e => e.SubastaId == subastaId && e.JobType == jobType);

            var entry = await _jobCollection.Find(filter).FirstOrDefaultAsync();

            return entry?.HangfireJobId;
        }
        /// <summary>
        /// Método que se encarga de eliminar un job en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subastaId">Parametro que corresponde al ID de la subasta</param>
        /// <param name="jobType">Parametro que corresponde al tipo de Job (Inicio, Fin, Pago)</param>
        public async Task EliminarJobIdAsync(Guid subastaId, string jobType)
        {
            var filter = Builders<SubastaJobMongo>.Filter.Where(
                e => e.SubastaId == subastaId && e.JobType == jobType);

            await _jobCollection.DeleteOneAsync(filter);
        }
    }
}
