using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class PrecioBaseProductoVO
    {
        public decimal precio { get; set; }

        public PrecioBaseProductoVO(decimal precio)
        {
            this.precio = precio;
        }
    }
}
