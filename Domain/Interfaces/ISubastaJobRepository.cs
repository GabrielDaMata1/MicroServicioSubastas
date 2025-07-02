using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISubastaJobRepository
    {
        Task GuardarJobAsync(Guid subastaId, string jobType, string hangfireJobId);
        Task<string> ObtenerJobIdAsync(Guid subastaId, string jobType);
        Task EliminarJobIdAsync(Guid subastaId, string jobType);
    }
}
