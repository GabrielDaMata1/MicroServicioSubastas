using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class IncrementoMinimoSubastaVO
    {
        public decimal incrementoMinimo { get; set; }

        public IncrementoMinimoSubastaVO(decimal incrementominimo)
        {
            incrementoMinimo= incrementominimo;
        }
    }
}
