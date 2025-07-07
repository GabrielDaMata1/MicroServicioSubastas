using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al intentar eliminar una subasta que ya ha comenzado.
    /// </summary>
    public class SubastaNoEliminableException: Exception
    {
        public SubastaNoEliminableException()
            : base("La subasta no puede ser eliminada porque ya ha comenzado.") { }
    }
}
