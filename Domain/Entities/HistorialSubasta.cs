using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    /// <summary>
    /// Clase Entity que representa a la entidad HistorialSubasta en el dominio del sistema.
    /// </summary>
    public class HistorialSubasta
    {
        /// <summary>
        /// Atributo que corresponde al ID del historial de subasta.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta.
        /// </summary>
        public Guid IdSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del usuario ganador de la subasta.
        /// </summary>
        public Guid IdUsuario { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto final de la subasta.
        /// </summary>
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
