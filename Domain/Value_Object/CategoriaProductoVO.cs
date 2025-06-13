using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class CategoriaProductoVO
    {
        public string categoria { get; set; }

        public CategoriaProductoVO(string categoria)
        {
            this.categoria = categoria;
        }
    }
}
