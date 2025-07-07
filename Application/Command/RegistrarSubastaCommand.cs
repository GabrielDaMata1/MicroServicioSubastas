using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    /// <summary>
    /// Clase Command que se encarga de enviar la solicitud para registrar una subasta.
    /// </summary>
    public class RegistrarSubastaCommand : IRequest<bool>
    {
        /// <summary>
        /// Atributo DTO que se encarga de recibir la información de la subasta a registrar.
        /// </summary>
        public RegistrarSubastaDTO SubastaDto;

        public RegistrarSubastaCommand(RegistrarSubastaDTO subastaDto)
        {
            this.SubastaDto = subastaDto;
        }
    }
}
