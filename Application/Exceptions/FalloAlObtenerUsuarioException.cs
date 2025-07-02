using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class FalloAlObtenerUsuarioException : Exception
    {
        public FalloAlObtenerUsuarioException() : base("Ha ocurrido un error al obtener el usuario.") { }

        public FalloAlObtenerUsuarioException(string mensaje) : base(mensaje) { }

        public FalloAlObtenerUsuarioException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
