using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPujaService
    {
        Task<Puja> ObtenerPujaGanadoraPorIdSubasta(Guid idSubasta);

        Task<List<Puja>> ObtenerPujasSubasta(Guid idSubasta);
    }
}
