using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Models.PostgreSQL;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PostgreSQL
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

        public async Task<HttpStatusCode> ModificarSubastaAsync(Subasta subasta, Guid IdUsuario)
        {
            var subastaModificar = await _dbContext.Set<SubastaPostgreSQL>()
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(subasta.Id.ToString())); ;

            if (subastaModificar == null)
                return HttpStatusCode.NotFound;

            subastaModificar.Id = subasta.Id;
            subastaModificar.Nombre = subasta.nombreSubasta.Nombre;
            subastaModificar.Descripcion = subasta.descripcionSubasta.descripcion;
            subastaModificar.idProducto = subasta.idProductoSubasta;
            subastaModificar.fechaInicio = subasta.fechaInicioSubasta.fechaInicio;
            subastaModificar.fechaFin = subasta.fechaFinSubasta.fechaFin;
            subastaModificar.IdUsuario = IdUsuario;
            subastaModificar.incrementoMinimo = subasta.incrementoMinimoSubasta.incrementoMinimo;
            subastaModificar.precioReserva = subasta.precioReservaSubasta.precioReserva;
            subastaModificar.Estado = subasta.estadoSubasta.estado;
            _dbContext.Set<SubastaPostgreSQL>().Update(subastaModificar);
            await _dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        public async Task<bool> EliminarSubastaAsync(Guid idSubasta)
        {
            var subasta = await _dbContext.Set<SubastaPostgreSQL>()
                .FirstOrDefaultAsync(p => p.Id == idSubasta);

            if (subasta == null)
                return false;

            _dbContext.Set<SubastaPostgreSQL>().Remove(subasta);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
