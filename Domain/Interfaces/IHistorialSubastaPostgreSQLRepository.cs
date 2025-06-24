using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHistorialSubastaPostgreSQLRepository
    {
        Task<Guid> registrarHistorialSubastaAsync(HistorialSubasta historialSubasta, string resultado);
    }
}
