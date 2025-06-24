using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PostgreSQL
{
    public class HistorialSubastasPostgreSQL
    {
        [Key]
        public Guid Id { get; set; }
        public Guid IdSubasta { get; set; }

        [ForeignKey("IdSubasta")]
        public virtual SubastaPostgreSQL Subasta { get; set; }

        [Required]
        public Guid IdUsuario { get; set; }

        [Required]
        public decimal MontoFinal { get; set; }

        [Required]
        public string Resultado { get; set; }
    }
}

