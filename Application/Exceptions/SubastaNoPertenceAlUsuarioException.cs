using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al intentar eliminar una subasta que no pertenece al usuario autenticado
    /// </summary>
    public class SubastaNoPertenceAlUsuarioException: Exception
    {
        public SubastaNoPertenceAlUsuarioException()
            : base("La subasta no puede ser eliminada porque no le pertenece al usuario autenticado.") { }
    }
}
