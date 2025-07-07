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
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre el historial de subastas almacenadas en PostgreSQL.
    /// </summary>
    public class HistorialSubastaPostgreSQLRepository: IHistorialSubastaPostgreSQLRepository

    {
        /// <summary>
        /// Atributo que corresponde al contexto de la base de datos del Microservicio Subasta en PostgreSQL.
        /// </summary>
        private readonly SubastaDbContext _dbContext;

        public HistorialSubastaPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Metodo que se encarga de registrar el historial de una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="historialSubasta">Parámetro que corresponde a un objeto HistorialSubasta con su detalle.</param>
        /// <param name="resultado">Parámetro que corresponde al resultado final de la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        public async Task<Guid> registrarHistorialSubastaAsync(HistorialSubasta historialSubasta, string resultado)
        {
            var historialSubastaBD = historialSubasta.ToPostgres(resultado);
            await _dbContext.AddAsync(historialSubastaBD);
            await _dbContext.SaveChangesAsync();
            return historialSubastaBD.Id;
        }

    }
}
