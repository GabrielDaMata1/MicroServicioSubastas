using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class SubastaNoModificableException : Exception
    {
        public SubastaNoModificableException()
            : base("La subasta no puede ser modificada porque ya ha comenzado.") { }
    }


}
