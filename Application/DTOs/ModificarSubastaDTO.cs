using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para modificar una subasta
    /// </summary>
    public class ModificarSubastaDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la subasta.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre de la subasta.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripcion de la subasta.
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de inicio de la subasta.
        /// </summary>
        public DateTime fechaInicio { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha fin de la subasta.
        /// </summary>
        public DateTime fechaFin { get; set; }
        /// <summary>
        /// Atributo que corresponde al incremento mínimo de la subasta.
        /// </summary>
        public decimal incrementoMinimo { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio de reserva de la subasta.
        /// </summary>
        public decimal precioReserva { get; set; }
        /// <summary>
        /// Atributo que corresponde al correo del subastador que organiza la subasta.
        /// </summary>
        public string correoUsuario { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del producto subastado en de la subasta.
        /// </summary>
        public Guid idProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta.
        /// </summary>
        public string estado { get; set; }
    }
}
