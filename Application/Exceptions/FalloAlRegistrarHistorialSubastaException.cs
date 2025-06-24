using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class FalloAlRegistrarHistorialSubastaException: Exception
    {
        public FalloAlRegistrarHistorialSubastaException() : base("Ha ocurrido un error al registrar el historial de subasta.") { }

        public FalloAlRegistrarHistorialSubastaException(string mensaje) : base(mensaje) { }

        public FalloAlRegistrarHistorialSubastaException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
