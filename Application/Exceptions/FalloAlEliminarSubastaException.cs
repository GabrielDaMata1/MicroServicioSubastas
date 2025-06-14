using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class FalloAlEliminarSubastaException : Exception
    {
        public FalloAlEliminarSubastaException() : base("Ha ocurrido un error al eliminar la subasta.") { }

        public FalloAlEliminarSubastaException(string mensaje) : base(mensaje) { }

        public FalloAlEliminarSubastaException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
