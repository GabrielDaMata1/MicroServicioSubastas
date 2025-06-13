using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Models.MongoDB
{
    public class SubastaMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("Nombre")]
        public string Nombre { get; set; }

        [BsonElement("Descripcion")]
        public string Descripcion { get; set; }

        [BsonElement("ProductoId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ProductoId { get; set; }

        [BsonElement("FechaInicio")]
        public DateTime FechaInicio { get; set; }

        [BsonElement("FechaFin")]
        public DateTime FechaFin { get; set; }

        [BsonElement("PrecioReserva")]
        public decimal PrecioReserva { get; set; }

        [BsonElement("IncrementoMinimo")]
        public decimal IncrementoMinimo { get; set; }

        [BsonElement("IdUsuario")]
        [BsonRepresentation(BsonType.String)]
        public Guid IdUsuario { get; set; }
    }
}
