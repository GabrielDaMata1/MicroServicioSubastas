using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class FalloAlRegistrarSubastaException : Exception
    {
        public FalloAlRegistrarSubastaException() : base("Ha ocurrido un error al registrar la subasta.") { }

        public FalloAlRegistrarSubastaException(string mensaje) : base(mensaje) { }

        public FalloAlRegistrarSubastaException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }


}
