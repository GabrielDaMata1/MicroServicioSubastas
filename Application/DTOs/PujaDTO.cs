using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Objects;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para consultar una puja.
    /// </summary>
    public class PujaDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la puja.
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del usuario.
        /// </summary>
        public Guid idUsuario { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta.
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
    }
}
