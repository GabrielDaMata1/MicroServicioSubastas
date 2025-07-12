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
    public class ModificarSubastaCommandTest
    {
        [Fact]
        public void Constructor_AsignaDTOCorrectamente()
        {
            var dto = new ModificarSubastaDTO
            {
                Id = Guid.NewGuid(),
                Nombre = "Subasta Diamante",
                Descripcion = "Productos tecnológicos de última generación",
                fechaInicio = DateTime.UtcNow.AddDays(1),
                fechaFin = DateTime.UtcNow.AddDays(10),
                incrementoMinimo = 100,
                precioReserva = 5000,
                correoUsuario = "subastador@correo.com",
                idProducto = Guid.NewGuid(),
                estado = "Programada"
            };

            var command = new ModificarSubastaCommand(dto);

            Assert.Equal(dto, command.subastaDto);
            Assert.Equal(dto.Id, command.subastaDto.Id);
            Assert.Equal(dto.Nombre, command.subastaDto.Nombre);
            Assert.Equal(dto.Descripcion, command.subastaDto.Descripcion);
            Assert.Equal(dto.fechaInicio, command.subastaDto.fechaInicio);
            Assert.Equal(dto.fechaFin, command.subastaDto.fechaFin);
            Assert.Equal(dto.incrementoMinimo, command.subastaDto.incrementoMinimo);
            Assert.Equal(dto.precioReserva, command.subastaDto.precioReserva);
            Assert.Equal(dto.correoUsuario, command.subastaDto.correoUsuario);
            Assert.Equal(dto.idProducto, command.subastaDto.idProducto);
            Assert.Equal(dto.estado, command.subastaDto.estado);
        }

        [Fact]
        public void ModificarSubastaCommand_ImplementaIRequestBool()
        {
            var command = new ModificarSubastaCommand(new ModificarSubastaDTO());

            Assert.IsAssignableFrom<IRequest<bool>>(command);
        }

    }
}
