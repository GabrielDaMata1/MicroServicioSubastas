using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al registrar una subasta en las bases de datos (PostgreSQL,MongoDB).
    /// </summary>
    public class FalloAlRegistrarSubastaException : Exception
    {
        public FalloAlRegistrarSubastaException() : base("Ha ocurrido un error al registrar la subasta.") { }

        public FalloAlRegistrarSubastaException(string mensaje) : base(mensaje) { }

        public FalloAlRegistrarSubastaException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }


}
