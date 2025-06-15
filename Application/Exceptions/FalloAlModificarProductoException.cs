using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class FalloAlModificarProductoException: Exception
    {
        public FalloAlModificarProductoException() : base("Ha ocurrido un error al modificar el producto en el Microservicio Producto.") { }

        public FalloAlModificarProductoException(string mensaje) : base(mensaje) { }

        public FalloAlModificarProductoException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
