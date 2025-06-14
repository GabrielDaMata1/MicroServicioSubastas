using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PostgreSQL
{
    public class SubastaPostgreSQL
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Estado { get; set; }

        [Required]
        public Guid idProducto { get; set; }

        [Required]
        public DateTime fechaInicio { get; set; }

        [Required]
        public DateTime fechaFin { get; set; }


        [Required]
        public decimal incrementoMinimo { get; set; }

        [Required]
        public decimal precioReserva { get; set; }

        [Required]
        public Guid IdUsuario { get; set; }
    }
}
