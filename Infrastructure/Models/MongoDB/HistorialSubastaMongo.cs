using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Models.MongoDB
{
    public class HistorialSubastaMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("IdSubasta")]
        [BsonRepresentation(BsonType.String)]
        public Guid IdSubasta { get; set; }

        [BsonElement("MontoFinal")]
        public decimal MontoFinal { get; set; }

        [BsonElement("IdUsuario")]
        [BsonRepresentation(BsonType.String)]
        public Guid IdUsuario { get; set; }
        [BsonElement("Resultado")]
        public string Resultado { get; set; }
    }
}
