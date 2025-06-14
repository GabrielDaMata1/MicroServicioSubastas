using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.MongoDB;
using Infrastructure.Models.PostgreSQL;

namespace Infrastructure.Mappers
{
    public static class SubastaMongoMapper
    {
        public static SubastaMongo ToMongo(this Subasta subasta, Guid idUsuario)
        {
            return new SubastaMongo
            {
                Id = subasta.Id,
                Nombre = subasta.nombreSubasta.Nombre,
                Descripcion = subasta.descripcionSubasta.descripcion,
                FechaFin = subasta.fechaFinSubasta.fechaFin,
                FechaInicio = subasta.fechaInicioSubasta.fechaInicio,
                ProductoId = subasta.idProductoSubasta,
                IdUsuario = idUsuario,
                IncrementoMinimo = subasta.incrementoMinimoSubasta.incrementoMinimo,
                PrecioReserva = subasta.precioReservaSubasta.precioReserva,
                Estado = subasta.estadoSubasta.estado
            };
        }

    }
}
