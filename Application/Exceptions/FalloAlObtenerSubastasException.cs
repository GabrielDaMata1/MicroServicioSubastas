using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class FalloAlObtenerSubastasException: Exception
    {
        public FalloAlObtenerSubastasException() : base("Ha ocurrido un error al obtener las subasta.") { }

        public FalloAlObtenerSubastasException(string mensaje) : base(mensaje) { }

        public FalloAlObtenerSubastasException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
