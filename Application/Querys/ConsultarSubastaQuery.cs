using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Querys
{
    /// <summary>
    /// Clase Query que se encarga de enviar la solicitud para consultar una subasta en específico.
    /// </summary>
    public class ConsultarSubastaQuery:IRequest<HistorialSubastasDTO>
    {
        /// <summary>
        /// Atributo DTO que contiene el ID de la subasta a consultar.
        /// </summary>
        public ConsultarSubastaDTO SubastaDto { get; set; }

        public ConsultarSubastaQuery(ConsultarSubastaDTO subastaDto)
        {
        SubastaDto = subastaDto;
        }
    }
}
