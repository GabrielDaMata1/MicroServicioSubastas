using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Objects
{
    public class MontoPredeterminadoPujaVO
    {
        public decimal montoPredeterminado { get; set; }

        public MontoPredeterminadoPujaVO(decimal montopredeterminado)
        {
            montoPredeterminado = montopredeterminado;
        }
    }
}
