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
    /// Clase Query que se encarga de enviar la solicitud para consultar las subastas ganadas por un usuario.
    /// </summary>
    public class ConsultarSubastasGanadasUsuarioQuery : IRequest<List<HistorialSubastasGanadasDTO>>
    {
        /// <summary>
        /// Atributo DTO que contiene el correo del usuario a consultar.
        /// </summary>
        public ConsultarSubastasUsuarioDTO SubastaDto { get; set; }

        public ConsultarSubastasGanadasUsuarioQuery(ConsultarSubastasUsuarioDTO subastaDto)
        {
            SubastaDto = subastaDto;
        }
    }
}
