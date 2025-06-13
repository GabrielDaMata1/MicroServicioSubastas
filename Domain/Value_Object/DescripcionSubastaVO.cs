using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class DescripcionSubastaVO
    {
        public string descripcion { get; set; }

        public DescripcionSubastaVO(string descripcion)
        {
            this.descripcion = descripcion;
        }
    }
}
