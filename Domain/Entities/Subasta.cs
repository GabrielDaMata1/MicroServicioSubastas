using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    public class Subasta
    {
        public Guid Id { get; set; }
        public NombreSubastaVO nombreSubasta { get; set; }
        public DescripcionSubastaVO descripcionSubasta { get; set; }
        public Guid idProductoSubasta { get; set; }
        public FechaInicioSubastaVO fechaInicioSubasta { get; set; }
        public FechaFinSubastaVO fechaFinSubasta { get; set; }
        public IncrementoMinimoSubastaVO incrementoMinimoSubasta { get; set; }
        public PrecioReservaSubastaVO precioReservaSubasta { get; set; }
        public EstadoSubastaVO estadoSubasta { get; set; }

        public Subasta(NombreSubastaVO nombreSubasta, DescripcionSubastaVO descripcionSubasta, Guid idProductoSubasta, FechaInicioSubastaVO fechaInicioSubasta, FechaFinSubastaVO fechaFinSubasta, IncrementoMinimoSubastaVO incrementoMinimoSubasta, PrecioReservaSubastaVO precioReservaSubasta)
        {
            Id = Guid.NewGuid();
            this.nombreSubasta = nombreSubasta;
            this.descripcionSubasta = descripcionSubasta;
            this.idProductoSubasta = idProductoSubasta;
            this.fechaInicioSubasta = fechaInicioSubasta;
            this.fechaFinSubasta = fechaFinSubasta;
            this.incrementoMinimoSubasta = incrementoMinimoSubasta;
            this.precioReservaSubasta = precioReservaSubasta;
            this.estadoSubasta = new EstadoSubastaVO("Pending");

        }
        [JsonConstructor]
        public Subasta(Guid id, NombreSubastaVO nombreSubasta, DescripcionSubastaVO descripcionSubasta, Guid idProductoSubasta, FechaInicioSubastaVO fechaInicioSubasta, FechaFinSubastaVO fechaFinSubasta, IncrementoMinimoSubastaVO incrementoMinimoSubasta, PrecioReservaSubastaVO precioReservaSubasta, EstadoSubastaVO estadoSubasta)
        {
            Id = id;
            this.nombreSubasta = nombreSubasta;
            this.descripcionSubasta = descripcionSubasta;
            this.idProductoSubasta = idProductoSubasta;
            this.fechaInicioSubasta = fechaInicioSubasta;
            this.fechaFinSubasta = fechaFinSubasta;
            this.incrementoMinimoSubasta = incrementoMinimoSubasta;
            this.precioReservaSubasta = precioReservaSubasta;
            this.estadoSubasta=estadoSubasta;
        }
    }

   
}
