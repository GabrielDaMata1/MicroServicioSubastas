using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al modificar una subasta en las bases de datos (PostgreSQL,MongoDB).
    /// </summary>
    public class FalloAlModificarSubastaException : Exception
    {
        public FalloAlModificarSubastaException() : base("Ha ocurrido un error al modificar la subasta.") { }

        public FalloAlModificarSubastaException(string mensaje) : base(mensaje) { }

        public FalloAlModificarSubastaException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
