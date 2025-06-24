using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;

namespace Domain.Factory
{
    public static class HistorialSubastaFactory
    {
        public static HistorialSubasta CrearHistorialSubasta(Guid idUsuario, Guid idSubasta, decimal montoGanador)
        {
            var montoGanadorVO = new MontoFinalSubastaVO(montoGanador);

            return new HistorialSubasta(idSubasta,idUsuario,montoGanadorVO);
        }

        public static HistorialSubasta CrearHistorialSubastaConID(Guid id, Guid idUsuario, Guid idSubasta, decimal montoGanador)
        {
            var montoGanadorVO = new MontoFinalSubastaVO(montoGanador);

            return new HistorialSubasta(id, idSubasta, idUsuario, montoGanadorVO);
        }
    }
}
