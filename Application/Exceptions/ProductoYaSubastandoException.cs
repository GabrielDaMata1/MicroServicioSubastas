using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al intentar registrar una subasta con un producto que ya está siendo subastado.
    /// </summary>
    public class ProductoYaSubastandoException: Exception
    {
        public ProductoYaSubastandoException()
            : base("El producto no puede ser subastado porque ya lo está siendo en otra subasta.") { }
    }
}
