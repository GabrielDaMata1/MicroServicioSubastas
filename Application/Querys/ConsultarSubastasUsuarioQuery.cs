using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Querys
{
    public class ConsultarSubastasUsuarioQuery : IRequest<List<HistorialSubastasDTO>>
    {
        public ConsultarSubastasUsuarioDTO SubastaDto { get; set; }

        public ConsultarSubastasUsuarioQuery(ConsultarSubastasUsuarioDTO subastaDto)
        {
            SubastaDto = subastaDto;
        }
    }
}
