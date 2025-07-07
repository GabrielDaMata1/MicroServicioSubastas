using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    /// <summary>
    /// Clase Entity que representa a la entidad Subasta en el dominio del sistema.
    /// </summary>
    public class Subasta
    {
        /// <summary>
        /// Atributo que corresponde al ID de la subasta.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre de la subasta.
        /// </summary>
        public NombreSubastaVO nombreSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripcion de la subasta.
        /// </summary>
        public DescripcionSubastaVO descripcionSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del producto subastando en la subasta.
        /// </summary>
        public Guid idProductoSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de inicio de la subasta.
        /// </summary>
        public FechaInicioSubastaVO fechaInicioSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha fin de la subasta.
        /// </summary>
        public FechaFinSubastaVO fechaFinSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al incremento minimo de la subasta.
        /// </summary>
        public IncrementoMinimoSubastaVO incrementoMinimoSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio de reserva de la subasta.
        /// </summary>
        public PrecioReservaSubastaVO precioReservaSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta.
        /// </summary>
        public EstadoSubastaVO estadoSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del subastador.
        /// </summary>
        public Guid idUsuario { get; set; }

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

        public Subasta(Guid id, NombreSubastaVO nombreSubasta, DescripcionSubastaVO descripcionSubasta, Guid idProductoSubasta, FechaInicioSubastaVO fechaInicioSubasta, FechaFinSubastaVO fechaFinSubasta, IncrementoMinimoSubastaVO incrementoMinimoSubasta, PrecioReservaSubastaVO precioReservaSubasta, EstadoSubastaVO estadoSubasta, Guid idUsuario)
        {
            Id = id;
            this.nombreSubasta = nombreSubasta;
            this.descripcionSubasta = descripcionSubasta;
            this.idProductoSubasta = idProductoSubasta;
            this.fechaInicioSubasta = fechaInicioSubasta;
            this.fechaFinSubasta = fechaFinSubasta;
            this.incrementoMinimoSubasta = incrementoMinimoSubasta;
            this.precioReservaSubasta = precioReservaSubasta;
            this.estadoSubasta = estadoSubasta;
            this.idUsuario = idUsuario;
        }
    }

   
}
