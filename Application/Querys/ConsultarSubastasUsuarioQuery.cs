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
    /// Clase Query que se encarga de enviar la solicitud para consultar las subastas organizadas por un subastador.
    /// </summary>
    public class ConsultarSubastasUsuarioQuery : IRequest<List<HistorialSubastasDTO>>
    {
        /// <summary>
        /// Atributo DTO que contiene el correo del subastador que organizó las subastas.
        /// </summary>
        public ConsultarSubastasUsuarioDTO SubastaDto { get; set; }

        public ConsultarSubastasUsuarioQuery(ConsultarSubastasUsuarioDTO subastaDto)
        {
            SubastaDto = subastaDto;
        }
    }
}
