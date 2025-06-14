using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class ProductoNoPerteneceAlUsuarioException : Exception
    {
        public ProductoNoPerteneceAlUsuarioException()
            : base("El producto no puede ser subastado porque no le pertenece al usuario autenticado.") { }
    }
}
