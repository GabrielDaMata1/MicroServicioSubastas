using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class ProductoYaSubastandoException: Exception
    {
        public ProductoYaSubastandoException()
            : base("El producto no puede ser subastado porque ya lo está siendo en otra subasta.") { }
    }
}
