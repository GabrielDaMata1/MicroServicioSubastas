using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public record SubastaIniciadaEvent(Guid SubastaId, DateTime fechaInicio, DateTime fechaFin);
}
