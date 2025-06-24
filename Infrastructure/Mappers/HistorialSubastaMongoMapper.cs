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
    public static class HistorialSubastaMongoMapper
    {
        public static HistorialSubastaMongo ToMongo(this HistorialSubasta historialSubasta, string resultado)
        {
            return new HistorialSubastaMongo
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
