using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    /// <summary>
    /// Clase Event que es consumida por un consumidor para que elimine la subasta en la base de datos en MongoDB
    /// </summary>
    public record SubastaEliminadaEvent(Guid idSubasta);
}
