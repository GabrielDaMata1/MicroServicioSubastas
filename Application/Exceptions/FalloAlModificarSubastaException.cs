using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class FalloAlModificarSubastaException : Exception
    {
        public FalloAlModificarSubastaException() : base("Ha ocurrido un error al modificar la subasta.") { }

        public FalloAlModificarSubastaException(string mensaje) : base(mensaje) { }

        public FalloAlModificarSubastaException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
