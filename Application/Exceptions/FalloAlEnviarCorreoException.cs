using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al enviar una notificación mediante correo electrónico.
    /// </summary>
    public class FalloAlEnviarCorreoException: System.Exception
    {
        public FalloAlEnviarCorreoException() : base("Ha ocurrido un error al enviar el correo.") { }

        public FalloAlEnviarCorreoException(string mensaje) : base(mensaje) { }

        public FalloAlEnviarCorreoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }
}
