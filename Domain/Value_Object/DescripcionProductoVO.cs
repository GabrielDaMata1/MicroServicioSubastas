using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class DescripcionProductoVO
    {
        public string descripcion { get; set; }

        public DescripcionProductoVO(string descripcion)
        {
            this.descripcion = descripcion;
        }
    }
}
