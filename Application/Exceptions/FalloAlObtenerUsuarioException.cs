using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al obtener un usuario en el Microservicio de Usuarios.
    /// </summary>
    public class FalloAlObtenerUsuarioException : Exception
    {
        public FalloAlObtenerUsuarioException() : base("Ha ocurrido un error al obtener el usuario.") { }

        public FalloAlObtenerUsuarioException(string mensaje) : base(mensaje) { }

        public FalloAlObtenerUsuarioException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
