using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para notificar al usuario sobre su victoria en una subasta
    /// </summary>
    public class CorreoGanadorSubastaDTO
    {
        /// <summary>
        /// Atributo que corresponde al usuario quién recibe el correo.
        /// </summary>
        public string Destinatario { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto con el que se ganó la subasta.
        /// </summary>
        public decimal MontoGanador { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre de la subasta en la que ganó.
        /// </summary>
        public string NombreSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre del producto subastadp.
        /// </summary>
        public string NombreProducto { get; set; }
    }
}
