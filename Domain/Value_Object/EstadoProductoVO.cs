using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class EstadoProductoVO
    {
        public string estadoProducto { get; set; }

        public EstadoProductoVO(string estadoProducto)
        {
            this.estadoProducto=estadoProducto;
        }
    }
}
