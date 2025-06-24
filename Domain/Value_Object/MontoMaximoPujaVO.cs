using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Objects
{
    public class MontoMaximoPujaVO
    {
        public decimal montoMaximo { get; set; }

        public MontoMaximoPujaVO(decimal montomaximo)
        {
            montoMaximo = montomaximo;
        }
    }
}
