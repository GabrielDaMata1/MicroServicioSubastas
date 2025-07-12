using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exceptions;
using Application.Handler;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Domain.Value_Object;
using MassTransit;
using Moq;

namespace TestMicroservicioSubasta.HandlerTest
{
    public class ModificarSubastaHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<ISubastaSchedule> _subastaScheduleMock = new();

        private readonly ModificarSubastaHandler _handler;

        public ModificarSubastaHandlerTest()
        {
            _handler = new ModificarSubastaHandler(
                _subastaServiceMock.Object,
                _publishEndpointMock.Object,
                _usuarioServiceMock.Object,
                _subastaScheduleMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarTrue_CuandoModificacionExitosa()
        {
            var subastaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var correo = "usuario@correo.com";

            var dto = new ModificarSubastaDTO
            {
                Id = subastaId,
                Nombre = "Subasta Actualizada",
                Descripcion = "Descripción modificada",
                fechaInicio = DateTime.UtcNow.AddDays(2),
                fechaFin = DateTime.UtcNow.AddDays(5),
                incrementoMinimo = 100,
                precioReserva = 3000,
                correoUsuario = correo,
                idProducto = productoId,
                estado = "Programada"
            };

            var command = new ModificarSubastaCommand(dto);

            var subastaBD = new Subasta(
                subastaId,
                new NombreSubastaVO("Original"),
                new DescripcionSubastaVO("Original"),
                productoId,
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(2)),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(5)),
                new IncrementoMinimoSubastaVO(50),
                new PrecioReservaSubastaVO(2000),
                new EstadoSubastaVO("Programada"),
                usuarioId
            );

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subastaBD);
            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.ModificarSubastaPostgreSQLAsync(It.IsAny<Subasta>(), usuarioId)).ReturnsAsync(HttpStatusCode.OK);
            _subastaScheduleMock.Setup(x => x.ReprogramarEventosDeSubasta(subastaId, dto.fechaInicio, dto.fechaFin)).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            _publishEndpointMock.Verify(x =>
                x.Publish(It.Is<SubastaModificadaEvent>(e => e.subasta.Id == subastaId && e.idUsuario == usuarioId), default),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarSubastaNoModificableException_CuandoFechaInicioYaPaso()
        {
            var command = new ModificarSubastaCommand(new ModificarSubastaDTO
            {
                Id = Guid.NewGuid(),
                correoUsuario = "usuario@correo.com",
                fechaInicio = DateTime.UtcNow.AddHours(-1)
            });

            var subastaBD = new Subasta(
                command.subastaDto.Id,
                new NombreSubastaVO("Iniciada"),
                new DescripcionSubastaVO("Ya comenzó"),
                Guid.NewGuid(),
                new FechaInicioSubastaVO(DateTime.UtcNow.AddHours(-1)),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(1)),
                new IncrementoMinimoSubastaVO(50),
                new PrecioReservaSubastaVO(1500),
                new EstadoSubastaVO("Activa"),
                Guid.NewGuid()
            );

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(command.subastaDto.Id)).ReturnsAsync(subastaBD);

            await Assert.ThrowsAsync<SubastaNoModificableException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarFalloAlModificarSubastaException_CuandoActualizacionFalla()
        {
            var subastaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var command = new ModificarSubastaCommand(new ModificarSubastaDTO
            {
                Id = subastaId,
                Nombre = "Nombre",
                Descripcion = "Descripción",
                fechaInicio = DateTime.UtcNow.AddDays(1),
                fechaFin = DateTime.UtcNow.AddDays(3),
                incrementoMinimo = 200,
                precioReserva = 4000,
                correoUsuario = "usuario@correo.com",
                idProducto = Guid.NewGuid(),
                estado = "Programada"
            });

            var subastaBD = new Subasta(
                subastaId,
                new NombreSubastaVO("Original"),
                new DescripcionSubastaVO("Original"),
                command.subastaDto.idProducto,
                new FechaInicioSubastaVO(command.subastaDto.fechaInicio),
                new FechaFinSubastaVO(command.subastaDto.fechaFin),
                new IncrementoMinimoSubastaVO(150),
                new PrecioReservaSubastaVO(2500),
                new EstadoSubastaVO("Programada"),
                usuarioId
            );

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subastaBD);
            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(command.subastaDto.correoUsuario)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.ModificarSubastaPostgreSQLAsync(It.IsAny<Subasta>(), usuarioId)).ReturnsAsync(HttpStatusCode.BadRequest);

            await Assert.ThrowsAsync<FalloAlModificarSubastaException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

    }
}
