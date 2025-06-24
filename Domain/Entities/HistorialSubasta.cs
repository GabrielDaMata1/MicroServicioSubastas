using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    public class HistorialSubasta
    {
        public Guid Id { get; set; }
        public Guid IdSubasta { get; set; }
        public Guid IdUsuario { get; set; }
        public MontoFinalSubastaVO MontoFinalSubasta { get; set; }

        public HistorialSubasta(Guid idSubasta, Guid idUsuario, MontoFinalSubastaVO montoFinal)
        {
            Id= Guid.NewGuid();
            IdSubasta= idSubasta;
            IdUsuario= idUsuario;
            MontoFinalSubasta= montoFinal;
        }

        public HistorialSubasta(Guid id, Guid idSubasta, Guid idUsuario, MontoFinalSubastaVO montoFinal)
        {
            Id = id;
            IdSubasta = idSubasta;
            IdUsuario = idUsuario;
            MontoFinalSubasta = montoFinal;
        }

    }
    
}
