using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Persistance;

namespace Infrastructure.Repositories.PostgreSQL
{
    public class HistorialSubastaPostgreSQLRepository: IHistorialSubastaPostgreSQLRepository

    {
        private readonly SubastaDbContext _dbContext;

        public HistorialSubastaPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> registrarHistorialSubastaAsync(HistorialSubasta historialSubasta, string resultado)
        {
            var historialSubastaBD = historialSubasta.ToPostgres(resultado);
            await _dbContext.AddAsync(historialSubastaBD);
            await _dbContext.SaveChangesAsync();
            return historialSubastaBD.Id;
        }

    }
}
