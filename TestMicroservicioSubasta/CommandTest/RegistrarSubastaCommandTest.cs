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
    public class RegistrarSubastaCommandTest
    {
        [Fact]
        public void Constructor_AsignaDTOCorrectamente()
        {
            var dto = new RegistrarSubastaDTO
            {
                Nombre = "Subasta de Arte",
                Descripcion = "Obras clásicas en exhibición",
                idProducto = Guid.NewGuid(),
                fechaInicio = DateTime.UtcNow.AddDays(2),
                fechaFin = DateTime.UtcNow.AddDays(10),
                incrementoMinimo = 200,
                precioReserva = 10000,
                correoUsuario = "curador@galeria.com"
            };

            var command = new RegistrarSubastaCommand(dto);

            Assert.Equal(dto, command.SubastaDto);
            Assert.Equal(dto.Nombre, command.SubastaDto.Nombre);
            Assert.Equal(dto.Descripcion, command.SubastaDto.Descripcion);
            Assert.Equal(dto.idProducto, command.SubastaDto.idProducto);
            Assert.Equal(dto.fechaInicio, command.SubastaDto.fechaInicio);
            Assert.Equal(dto.fechaFin, command.SubastaDto.fechaFin);
            Assert.Equal(dto.incrementoMinimo, command.SubastaDto.incrementoMinimo);
            Assert.Equal(dto.precioReserva, command.SubastaDto.precioReserva);
            Assert.Equal(dto.correoUsuario, command.SubastaDto.correoUsuario);
        }

        [Fact]
        public void RegistrarSubastaCommand_ImplementaIRequestBool()
        {
            var command = new RegistrarSubastaCommand(new RegistrarSubastaDTO());

            Assert.IsAssignableFrom<IRequest<bool>>(command);
        }

    }
}
