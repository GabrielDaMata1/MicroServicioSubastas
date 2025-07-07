using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para consultar las subastas de un subastador.
    /// </summary>
    public class ConsultarSubastasUsuarioDTO
    {
        /// <summary>
        /// Atributo que corresponde al correo del subastador al que le pertenecen las subastas.
        /// </summary>
        public string correoUsuario { get; set; }
    }
}
