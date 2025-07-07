using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    /// <summary>
    /// Clase Event que es consumida por un consumidor para que modifique el estado de una subasta a "Ended" y se registre el historial de la subasta
    /// </summary>
    public record SubastaAcabadaEvent(Guid SubastaId);
}
