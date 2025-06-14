using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class SubastaNoPertenceAlUsuarioException: Exception
    {
        public SubastaNoPertenceAlUsuarioException()
            : base("La subasta no puede ser eliminada porque no le pertenece al usuario autenticado.") { }
    }
}
