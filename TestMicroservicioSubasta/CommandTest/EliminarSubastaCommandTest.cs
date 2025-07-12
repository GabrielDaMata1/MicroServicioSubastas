using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using MediatR;

namespace TestMicroservicioSubasta.CommandTest
{
    public class EliminarSubastaCommandTest
    {
        [Fact]
        public void Constructor_DeberiaAsignarDTOCorrectamente()
        {
            var dto = new EliminarSubastaDTO
            {
                idSubasta = Guid.NewGuid(),
                correoUsuario = "correoPrueba@gmail.com"
            };

            var command = new EliminarSubastaCommand(dto);

            Assert.Equal(dto, command.subastaDto);
            Assert.Equal(dto.idSubasta, command.subastaDto.idSubasta);
            Assert.Equal(dto.correoUsuario, command.subastaDto.correoUsuario);
        }

        [Fact]
        public void DebeImplementarIRequestBool()
        {
            var dto = new EliminarSubastaDTO();
            var command = new EliminarSubastaCommand(dto);

            Assert.IsAssignableFrom<IRequest<bool>>(command);
        }

    }
}
