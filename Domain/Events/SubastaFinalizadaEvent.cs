using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public record SubastaFinalizadaEvent(Guid SubastaId, DateTime fechaInicio, DateTime fechaFin);

}
