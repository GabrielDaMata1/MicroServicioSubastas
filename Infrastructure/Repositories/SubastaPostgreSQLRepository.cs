using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Persistance;

namespace Infrastructure.Repositories
{
    public class SubastaPostgreSQLRepository : ISubastaRepositoryPostgreSQL
    {
        private readonly SubastaDbContext _dbContext;

        public SubastaPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> RegistrarSubastaAsync(Subasta subasta, Guid IdUsuario)
        {
            var subastaBD = subasta.ToPostgres(IdUsuario);
            await _dbContext.Subasta.AddAsync(subastaBD);
            await _dbContext.SaveChangesAsync();
            return subastaBD.Id;
        }
    }
}
