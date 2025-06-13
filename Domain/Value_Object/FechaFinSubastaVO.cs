using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class FechaFinSubastaVO
    {
        public DateTime fechaFin { get; set; }

        [JsonConstructor]
        public FechaFinSubastaVO(DateTime fechafin)
        {
            fechaFin = fechafin;
        }
    }
}
