using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Objects
{
    public class TipoPujaVO
    {
        public string tipoPuja { get; set; }

        public TipoPujaVO(string tipopuja)
        {
            tipoPuja = tipopuja;
        }
    }
}
