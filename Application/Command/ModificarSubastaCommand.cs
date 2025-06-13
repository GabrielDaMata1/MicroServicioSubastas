using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class ModificarSubastaCommand : IRequest<bool>
    {
        public ModificarSubastaDTO subastaDto;

        public ModificarSubastaCommand(ModificarSubastaDTO subastaDto)
        {
            this.subastaDto = subastaDto;
        }
    }
}

