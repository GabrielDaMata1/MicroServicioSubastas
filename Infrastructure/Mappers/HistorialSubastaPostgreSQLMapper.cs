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
    /// Clase mapper que se encarga de mapear el objeto de tipo Entidad HistorialSubasta (Dominio) a una entidad en la base de datos en PostgreSQL
    /// </summary>
    public static class HistorialSubastaPostgreSQLMapper
    {
        /// <summary>
        /// Método que se encarga de mapear una HistorialSubasta (Entidad) a un modelo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="historialSubasta">Entidad que contiene los valores del historial de subasta a registrar</param>
        /// <param name="resultado">Parámetro que contiene el resultado de la subasta (Ganador, No Ganador) </param>
        /// <returns>Retorna un objeto de tipo HistorialSubastaPostgreSQL, que corresponde al modelo de historial de subasta en la base de datos en PostgreSQL.</returns>
        public static HistorialSubastasPostgreSQL ToPostgres(this HistorialSubasta historialSubasta, string resultado)
        {
            return new HistorialSubastasPostgreSQL
            {
                Id = historialSubasta.Id,
                IdUsuario = historialSubasta.IdUsuario,
                IdSubasta = historialSubasta.IdSubasta,
                MontoFinal = historialSubasta.MontoFinalSubasta.montoFinal,
                Resultado = resultado
            };
        }
    }
}
