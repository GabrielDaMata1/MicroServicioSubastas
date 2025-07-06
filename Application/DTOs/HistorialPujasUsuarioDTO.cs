using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class HistorialPujasUsuarioDTO
    {
        public Guid IdSubasta { get; set; }
        public Guid IdUsuario { get; set; }
        public string NombreSubasta { get; set; }
        public string DescripcionSubasta { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal IncrementoMinimo { get; set; }
        public decimal PrecioReserva { get; set; }

        public Guid IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public decimal PrecioBase { get; set; }
        public string Categoria { get; set; }
        public string UrlImagen { get; set; }

        public List<PujaUsuarioDTO> Pujas { get; set; } = new();

    }
}
