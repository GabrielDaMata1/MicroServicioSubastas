using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Objects
{
    public class MontoPujaVO
    {
        public decimal montoPuja { get; set; }

        public MontoPujaVO(decimal montopuja)
        {
            montoPuja = montopuja;
        }
    }
}
