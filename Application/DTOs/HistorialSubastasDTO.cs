using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class HistorialSubastasDTO
    {
        public Guid IdSubasta { get; set; }
        public string NombreSubasta { get; set; }
        public string DescripcionSubasta { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal incrementoMinimo { get; set; }

        public decimal precioReserva { get; set; }

        public Guid IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public decimal PrecioBase { get; set; }
        public string Categoria { get; set; }
    }
}
