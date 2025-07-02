using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Models.MongoDB
{
    public class SubastaJobMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("subastaId")]
        [BsonRepresentation(BsonType.String)]

        public Guid SubastaId { get; set; }

        [BsonElement("jobType")]
        public string JobType { get; set; }

        [BsonElement("hangfireJobId")]
        public string HangfireJobId { get; set; }
    }
}
