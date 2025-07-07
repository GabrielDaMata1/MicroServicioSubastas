using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Objects;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    /// Clase Entity que representa a la entidad Puja en el dominio del sistema.
    /// </summary>
    public class Puja
    {
        /// <summary>
        /// Atributo que corresponde al ID de la puja .
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del usuario que realizó la puja .
        /// </summary>
        public Guid IdUsuario { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta donde se realizó la puja .
        /// </summary>
        public Guid IdSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto de la puja .
        /// </summary>
        public MontoPujaVO MontoPuja { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto maximo de la puja .
        /// </summary>
        public MontoMaximoPujaVO MontoMaximo { get; set; }
        /// <summary>
        /// Atributo que corresponde al tipo de la puja .
        /// </summary>
        public TipoPujaVO TipoPuja { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto predeterminado de la puja .
        /// </summary>
        public MontoPredeterminadoPujaVO MontoPredeterminado { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha donde se registró la puja .
        /// </summary>
        public FechaPujaVO FechaPuja { get; set; }
        public Puja(Guid idUsuario, Guid idSubasta, MontoPujaVO montoPuja, MontoMaximoPujaVO montoMaximo, TipoPujaVO tipoPuja, MontoPredeterminadoPujaVO montoPredeterminado)
        {
            Id = Guid.NewGuid();
            IdUsuario = idUsuario;
            IdSubasta = idSubasta;
            MontoPuja = montoPuja;
            MontoMaximo = montoMaximo;
            TipoPuja = tipoPuja;
            MontoPredeterminado = montoPredeterminado;
        }
        [JsonConstructor]
        public Puja(Guid id, Guid idUsuario, Guid idSubasta, MontoPujaVO montoPuja, MontoMaximoPujaVO montoMaximo, TipoPujaVO tipoPuja, MontoPredeterminadoPujaVO montoPredeterminado)
        {
            Id = id;
            IdUsuario = idUsuario;
            IdSubasta = idSubasta;
            MontoPuja = montoPuja;
            MontoMaximo = montoMaximo;
            TipoPuja = tipoPuja;
            MontoPredeterminado = montoPredeterminado;
        }

        public Puja(Guid id, Guid idUsuario, Guid idSubasta, MontoPujaVO montoPuja, MontoMaximoPujaVO montoMaximo, TipoPujaVO tipoPuja, MontoPredeterminadoPujaVO montoPredeterminado, FechaPujaVO fechaPuja)
        {
            Id = id;
            IdUsuario = idUsuario;
            IdSubasta = idSubasta;
            MontoPuja = montoPuja;
            MontoMaximo = montoMaximo;
            TipoPuja = tipoPuja;
            MontoPredeterminado = montoPredeterminado;
            FechaPuja = fechaPuja;
        }
    }


}
