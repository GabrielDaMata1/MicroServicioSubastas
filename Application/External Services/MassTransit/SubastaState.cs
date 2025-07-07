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
    /// <summary>
    /// Clase de MassTransit necesaria para la máquina de estados que define los campos que manejará la base de datos de MassTransit en MongoDB.
    /// </summary>

    public class SubastaState : SagaStateMachineInstance, ISagaVersion
    {
        /// <summary>
        /// Atributo que representa el ID de correlación de la saga. Se utiliza para relacionar los eventos con los estados.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public Guid CorrelationId { get; set; }
        /// <summary>
        /// Atributo que corresponde a la versión de la Saga.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado actual de la subasta según la Saga.
        /// </summary>
        public string CurrentState { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta asociada a la Saga.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public Guid SubastaId { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de inicio de la subasta.
        /// </summary>
        public DateTime FechaInicio { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha fin de la subasta.
        /// </summary>
        public DateTime FechaFin { get; set; }

    }


}
