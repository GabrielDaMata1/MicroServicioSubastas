using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegistrarSubastaDTO
    {
        
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public Guid idProducto { get; set; }

        public DateTime fechaInicio { get; set; }

        public DateTime fechaFin { get; set; }


        public decimal incrementoMinimo { get; set; }

        public decimal precioReserva { get; set; }

        public string correoUsuario { get; set; }
    }
}
