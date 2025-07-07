using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al intentar registrar una subasta con un producto que no pertenece al usuario autenticado.
    /// </summary>
    public class ProductoNoPerteneceAlUsuarioException : Exception
    {
        public ProductoNoPerteneceAlUsuarioException()
            : base("El producto no puede ser subastado porque no le pertenece al usuario autenticado.") { }
    }
}
