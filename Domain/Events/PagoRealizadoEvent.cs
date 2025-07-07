using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Events
{
    /// <summary>
    /// Clase Event que es consumida por un consumidor para que la máquina de estados modifique el estado de una subasta a "Canceled"
    /// </summary>
    public record PagoRealizadoEvent(Guid idSubasta, Guid idUsuario);
}
