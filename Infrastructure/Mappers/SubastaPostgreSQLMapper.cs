using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.PostgreSQL;

namespace Infrastructure.Mappers
{
    public static class SubastaPostgreSQLMapper
    {
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
                precioReserva = subasta.precioReservaSubasta.precioReserva
            };
        }

    }
}
