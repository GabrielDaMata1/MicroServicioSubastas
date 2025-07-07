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
    /// <summary>
    /// Clase mapper que se encarga de mapear el objeto de tipo Entidad Subasta (Dominio) a una entidad en la base de datos en MongoDB
    /// </summary>
    public static class SubastaMongoMapper
    {

        /// <summary>
        /// Método que se encarga de mapear una Subasta (Entidad) a un modelo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="subasta">Entidad que contiene los valores de la subasta registrar</param>
        /// <param name="idUsuario">Parámetro que contiene el ID del subastador </param>
        /// <returns>Retorna un objeto de tipo SubastaMongoDB, que corresponde al modelo de historial de subasta en la base de datos en MongoDB.</returns>
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
