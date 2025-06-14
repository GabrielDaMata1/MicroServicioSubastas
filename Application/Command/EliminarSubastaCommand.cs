using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class EliminarSubastaCommand : IRequest<bool>
    {
        public EliminarSubastaDTO subastaDto;

        public EliminarSubastaCommand(EliminarSubastaDTO subastaDto)
        {
            this.subastaDto = subastaDto;
        }
    }
}
