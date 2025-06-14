using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class SubastaNoEliminableException: Exception
    {
        public SubastaNoEliminableException()
            : base("La subasta no puede ser eliminada porque ya ha comenzado.") { }
    }
}
