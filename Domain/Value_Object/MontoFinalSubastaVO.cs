using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class MontoFinalSubastaVO
    {
        public decimal montoFinal { get; set; }

        public MontoFinalSubastaVO(decimal montofinal)
        {
            montoFinal = montofinal;
        }
    }
}
