using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para consultar una subasta.
    /// </summary>
    public class ConsultarSubastaDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la subasta a consultar.
        /// </summary>
        public Guid idSubasta { get; set; }
    }
}
