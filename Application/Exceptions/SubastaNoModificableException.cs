using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al intentar modificar una subasta que ya ha comenzado.
    /// </summary>
    public class SubastaNoModificableException : Exception
    {
        public SubastaNoModificableException()
            : base("La subasta no puede ser modificada porque ya ha comenzado.") { }
    }


}
