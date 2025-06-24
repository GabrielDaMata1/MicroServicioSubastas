using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.External_Services.MassTransit
{
    public class SubastaState : SagaStateMachineInstance, ISagaVersion
    {
        [BsonRepresentation(BsonType.String)]
        public Guid CorrelationId { get; set; }

        public int Version { get; set; }

        public string CurrentState { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid SubastaId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

    }


}
