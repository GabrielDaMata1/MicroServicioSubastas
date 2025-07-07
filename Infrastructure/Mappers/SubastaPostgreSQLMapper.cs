using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.PostgreSQL;

namespace Infrastructure.Mappers
{
    /// <summary>
    /// Clase mapper que se encarga de mapear el objeto de tipo Entidad Subasta (Dominio) a una entidad en la base de datos en PostgreSQL
    /// </summary>
    public static class SubastaPostgreSQLMapper
    {

        /// <summary>
        /// Método que se encarga de mapear una Subasta (Entidad) a un modelo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="subasta">Entidad que contiene los valores de la subasta registrar</param>
        /// <param name="idUsuario">Parámetro que contiene el ID del subastador </param>
        /// <returns>Retorna un objeto de tipo SubastaPostgreSQL, que corresponde al modelo de historial de subasta en la base de datos en PostgreSQL.</returns>
        public static SubastaPostgreSQL ToPostgres(this Subasta subasta, Guid idUsuario)
        {
            return new SubastaPostgreSQL
            {
                Id = subasta.Id,
                Nombre = subasta.nombreSubasta.Nombre,
                Descripcion = subasta.descripcionSubasta.descripcion,
                fechaFin = subasta.fechaFinSubasta.fechaFin,
                fechaInicio = subasta.fechaInicioSubasta.fechaInicio,
                idProducto = subasta.idProductoSubasta,
                IdUsuario = idUsuario,
                incrementoMinimo = subasta.incrementoMinimoSubasta.incrementoMinimo,
                precioReserva = subasta.precioReservaSubasta.precioReserva,
                Estado = subasta.estadoSubasta.estado
            };
        }

    }
}
