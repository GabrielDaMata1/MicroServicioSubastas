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
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre las subastas almacenadas en PostgreSQL.
    /// </summary>
    public class SubastaPostgreSQLRepository : ISubastaRepositoryPostgreSQL
    {
        /// <summary>
        /// Atributo que corresponde al contexto de la base de datos del Microservicio Subasta en PostgreSQL.
        /// </summary>
        private readonly SubastaDbContext _dbContext;

        public SubastaPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Metodo que se encarga de registrar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que registra la subasta.</param>
        public async Task<Guid> RegistrarSubastaAsync(Subasta subasta, Guid IdUsuario)
        {
            var subastaBD = subasta.ToPostgres(IdUsuario);
            await _dbContext.Subasta.AddAsync(subastaBD);
            await _dbContext.SaveChangesAsync();
            return subastaBD.Id;
        }

        /// <summary>
        /// Metodo que se encarga de modificar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Parámetro que corresponde a un objeto Subasta con su detalle.</param>
        /// <param name="IdUsuario">Parámetro que corresponde al ID del subastador que modifica la subasta.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
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

        /// <summary>
        /// Metodo que se encarga de eliminar una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la Subasta a eliminar.</param>
        /// <returns>Retorna un valor booleano si la operación ocurrió correctamente</returns>
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

        /// <summary>
        /// Metodo que se encarga de modificar el estado de una subasta en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idSubasta">Parámetro que corresponde al ID de la subasta a modificar.</param>
        /// <param name="nuevoEstado">Parámetro que corresponde al nuevo estado de la subasta a asignar.</param>
        /// <returns>Retorna uns estado HTTP exitoso si la operación ocurrió correctamente</returns>
        public async Task<HttpStatusCode> ActualizarEstadoSubasta(Guid idSubasta, string nuevoEstado)
        {
            var subasta = await _dbContext.Subasta.FindAsync(idSubasta);
            subasta.Estado = nuevoEstado;
            await _dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
