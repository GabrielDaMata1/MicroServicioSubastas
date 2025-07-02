using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Querys
{
    public class ConsultarSubastasGanadasUsuarioQuery : IRequest<List<HistorialSubastasGanadasDTO>>
    {
        public ConsultarSubastasUsuarioDTO SubastaDto { get; set; }

        public ConsultarSubastasGanadasUsuarioQuery(ConsultarSubastasUsuarioDTO subastaDto)
        {
            SubastaDto = subastaDto;
        }
    }
}
