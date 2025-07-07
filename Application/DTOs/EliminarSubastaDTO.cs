using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para eliminar una subasta de un subastador.
    /// </summary>
    public class EliminarSubastaDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la subasta a eliminar.
        /// </summary>
        public Guid idSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al correo del subastador al que le pertenece las subasta.
        /// </summary>
        public string correoUsuario { get; set; }
    }
}
