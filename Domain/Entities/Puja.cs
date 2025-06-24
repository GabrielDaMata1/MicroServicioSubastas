using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Objects;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Puja
    {
        public Guid Id { get; set; }
        public Guid IdUsuario { get; set; }

        public Guid IdSubasta { get; set; }

        public MontoPujaVO MontoPuja { get; set; }

        public MontoMaximoPujaVO MontoMaximo { get; set; }

        public TipoPujaVO TipoPuja { get; set; }

        public MontoPredeterminadoPujaVO MontoPredeterminado { get; set; }
        public Puja(Guid idUsuario, Guid idSubasta, MontoPujaVO montoPuja, MontoMaximoPujaVO montoMaximo, TipoPujaVO tipoPuja, MontoPredeterminadoPujaVO montoPredeterminado)
        {
            Id = Guid.NewGuid();
            IdUsuario = idUsuario;
            IdSubasta = idSubasta;
            MontoPuja = montoPuja;
            MontoMaximo = montoMaximo;
            TipoPuja = tipoPuja;
            MontoPredeterminado= montoPredeterminado;
        }
        [JsonConstructor]
        public Puja (Guid id, Guid idUsuario, Guid idSubasta, MontoPujaVO montoPuja, MontoMaximoPujaVO montoMaximo, TipoPujaVO tipoPuja, MontoPredeterminadoPujaVO montoPredeterminado)
        {
            Id = id;
            IdUsuario = idUsuario;
            IdSubasta = idSubasta;
            MontoPuja = montoPuja;
            MontoMaximo = montoMaximo;
            TipoPuja = tipoPuja;
            MontoPredeterminado=montoPredeterminado;
        }
    }

    
}
