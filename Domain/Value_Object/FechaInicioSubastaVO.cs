using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class FechaInicioSubastaVO
    {
        public DateTime fechaInicio { get; set; }

        [JsonConstructor]
        public FechaInicioSubastaVO(DateTime fechainicio)
        {
            fechaInicio=fechainicio;
        }
    }
}
