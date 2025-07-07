using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para consultar las pujas de un usuario en una subasta en el Microservicio Puja.
    /// </summary>
    public class PujaUsuarioDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la puja.
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// Atributo que corresponde al correo del usuario.
        /// </summary>
        public string correoUsuario { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta donde se hizo la puja.
        /// </summary>
        public Guid idSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto de la puja.
        /// </summary>
        public decimal montoPuja { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto maximo de la puja.
        /// </summary>
        public decimal montoMaximo { get; set; }
        /// <summary>
        /// Atributo que corresponde al tipo de puja.
        /// </summary>
        public string tipoPuja { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto predeterminado de la puja.
        /// </summary>
        public decimal montoPredeterminado { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha en que se realizó la puja.
        /// </summary>

        public DateTime fecha { get; set; }
    }
}
