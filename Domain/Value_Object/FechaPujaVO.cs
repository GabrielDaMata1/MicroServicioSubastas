using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Objects
{
    public class FechaPujaVO
    {
        public DateTime fechaPuja { get; set; }

        public FechaPujaVO(DateTime fechaPuja) { 
            this.fechaPuja = fechaPuja;
        }    
    }
}
