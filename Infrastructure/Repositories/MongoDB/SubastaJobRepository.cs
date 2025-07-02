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
    public class SubastaJobRepository : ISubastaJobRepository
    {
        private readonly IMongoCollection<SubastaJobMongo> _jobCollection;



        public SubastaJobRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("HangFireBD");
            _jobCollection = database.GetCollection<SubastaJobMongo>("SubastaJobs");
        }
        public async Task GuardarJobAsync(Guid subastaId, string jobType, string hangfireJobId)
        {
            var filter = Builders<SubastaJobMongo>.Filter.Where(
                e => e.SubastaId == subastaId && e.JobType == jobType);

            var update = Builders<SubastaJobMongo>.Update
                .Set(e => e.HangfireJobId, hangfireJobId);

            var options = new UpdateOptions { IsUpsert = true };

            await _jobCollection.UpdateOneAsync(filter, update, options);
        }

        public async Task<string> ObtenerJobIdAsync(Guid subastaId, string jobType)
        {
            var filter = Builders<SubastaJobMongo>.Filter.Where(
                e => e.SubastaId == subastaId && e.JobType == jobType);

            var entry = await _jobCollection.Find(filter).FirstOrDefaultAsync();

            return entry?.HangfireJobId;
        }

        public async Task EliminarJobIdAsync(Guid subastaId, string jobType)
        {
            var filter = Builders<SubastaJobMongo>.Filter.Where(
                e => e.SubastaId == subastaId && e.JobType == jobType);

            await _jobCollection.DeleteOneAsync(filter);
        }
    }
}
