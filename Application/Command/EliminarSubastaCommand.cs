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
    /// Clase Command que se encarga de enviar la solicitud para eliminaar una subasta que no ha empezado.
    /// </summary>
    public class EliminarSubastaCommand : IRequest<bool>
    {
        /// <summary>
        /// Atributo DTO que se encarga de recibir la información de la subasta a eliminar.
        /// </summary>
        public EliminarSubastaDTO subastaDto;

        public EliminarSubastaCommand(EliminarSubastaDTO subastaDto)
        {
            this.subastaDto = subastaDto;
        }
    }
}
