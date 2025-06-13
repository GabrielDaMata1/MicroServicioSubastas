using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class RegistrarSubastaCommand : IRequest<bool>
    {
        public RegistrarSubastaDTO SubastaDto;

        public RegistrarSubastaCommand(RegistrarSubastaDTO subastaDto)
        {
            this.SubastaDto = subastaDto;
        }
    }
}
