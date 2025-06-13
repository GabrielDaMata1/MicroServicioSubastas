using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class PrecioReservaSubastaVO
    {
        public decimal precioReserva { get; set; }

        public PrecioReservaSubastaVO(decimal precioreserva)
        {
            precioReserva= precioreserva;  
        }
    }
}
