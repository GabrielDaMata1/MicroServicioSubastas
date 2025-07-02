using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISubastaSchedule
    {
        Task ProgramarEventosDeSubasta(Guid subastaId, DateTime fechaInicio, DateTime fechaFin);
        Task PublicarInicio(Guid subastaId, DateTime fechaInicio, DateTime fechaFin);
        Task PublicarFin(Guid subastaId, DateTime fechaInicio, DateTime fechaFin);

        Task ReprogramarEventosDeSubasta(Guid subastaId, DateTime nuevaFechaInicio, DateTime nuevaFechaFin);

        Task EliminarTemporizadorPagoSubasta(Guid subastaId);
    }
}
