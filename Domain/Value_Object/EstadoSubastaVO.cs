using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class EstadoSubastaVO
    {
        public string estado { get; set; }
        public EstadoSubastaVO(string estado)
        {
            this.estado = estado;
        }
    }
}
