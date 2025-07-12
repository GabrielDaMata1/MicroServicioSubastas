using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EliminarSubastaHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();
        private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();

        private readonly EliminarSubastaHandler _handler;

        public EliminarSubastaHandlerTest()
        {
            _handler = new EliminarSubastaHandler(
                _subastaServiceMock.Object,
                _publishEndpointMock.Object,
                _usuarioServiceMock.Object,
                _productoServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarTrue_CuandoEliminacionExitosa()
        {
            var correo = "usuario@correo.com";
            var subastaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var productoId = Guid.NewGuid();

            var command = new EliminarSubastaCommand(new EliminarSubastaDTO
            {
                idSubasta = subastaId,
                correoUsuario = correo
            });

            var subasta = new Subasta(
                subastaId,
                new NombreSubastaVO("Subasta Test"),
                new DescripcionSubastaVO("Prueba de eliminación"),
                productoId,
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(2)),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(5)),
                new IncrementoMinimoSubastaVO(50),
                new PrecioReservaSubastaVO(1000),
                new EstadoSubastaVO("Programada"),
                usuarioId
            );

            var producto = new Producto(
                productoId,
                new NombreProductoVO("Producto Prueba"),
                new DescripcionProductoVO("Prueba eliminable"),
                new ImagenURLProductoVO("https://img.com/prod.jpg"),
                new PrecioBaseProductoVO(900),
                new CategoriaProductoVO("Pruebas"),
                new EstadoProductoVO("Subastado")
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subasta);
            _subastaServiceMock.Setup(x => x.ObtenerUsuarioIdPorSubastaIdMongoAsync(subastaId)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.EliminarSubastaPostgreSQLAsync(subastaId)).ReturnsAsync(true);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);
            _productoServiceMock.Setup(x => x.ModificarProductoDisponibleAsync(correo, producto)).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            _publishEndpointMock.Verify(x =>
                x.Publish(It.Is<SubastaEliminadaEvent>(e => e.idSubasta == subastaId), default),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarSubastaNoPertenceAlUsuarioException_CuandoUsuarioNoEsPropietario()
        {
            var command = new EliminarSubastaCommand(new EliminarSubastaDTO
            {
                idSubasta = Guid.NewGuid(),
                correoUsuario = "otro@correo.com"
            });

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(command.subastaDto.correoUsuario))
                .ReturnsAsync(Guid.NewGuid());

            _subastaServiceMock.Setup(x => x.ObtenerUsuarioIdPorSubastaIdMongoAsync(command.subastaDto.idSubasta))
                .ReturnsAsync(Guid.NewGuid());

            await Assert.ThrowsAsync<SubastaNoPertenceAlUsuarioException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarSubastaNoEliminableException_CuandoFechaInicioYaPaso()
        {
            var command = new EliminarSubastaCommand(new EliminarSubastaDTO
            {
                idSubasta = Guid.NewGuid(),
                correoUsuario = "usuario@correo.com"
            });

            var subasta = new Subasta(
                command.subastaDto.idSubasta,
                new NombreSubastaVO("Iniciada"),
                new DescripcionSubastaVO("Ya comenzó"),
                Guid.NewGuid(),
                new FechaInicioSubastaVO(DateTime.UtcNow.AddMinutes(-1)),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(1)),
                new IncrementoMinimoSubastaVO(50),
                new PrecioReservaSubastaVO(500),
                new EstadoSubastaVO("Activa"),
                Guid.NewGuid()
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(command.subastaDto.correoUsuario))
                .ReturnsAsync(subasta.idUsuario);

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(command.subastaDto.idSubasta))
                .ReturnsAsync(subasta);

            _subastaServiceMock.Setup(x => x.ObtenerUsuarioIdPorSubastaIdMongoAsync(command.subastaDto.idSubasta))
                .ReturnsAsync(subasta.idUsuario);

            await Assert.ThrowsAsync<SubastaNoEliminableException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarFalloAlEliminarSubastaException_CuandoEliminacionFalla()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var correo = "usuario@correo.com";

            var command = new EliminarSubastaCommand(new EliminarSubastaDTO
            {
                idSubasta = subastaId,
                correoUsuario = correo
            });

            var subasta = new Subasta(
                subastaId,
                new NombreSubastaVO("Subasta Fallida"),
                new DescripcionSubastaVO("No se pudo eliminar"),
                productoId,
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(3)),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(5)),
                new IncrementoMinimoSubastaVO(25),
                new PrecioReservaSubastaVO(1500),
                new EstadoSubastaVO("Programada"),
                usuarioId
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subasta);
            _subastaServiceMock.Setup(x => x.ObtenerUsuarioIdPorSubastaIdMongoAsync(subastaId)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.EliminarSubastaPostgreSQLAsync(subastaId)).ReturnsAsync(false);

            await Assert.ThrowsAsync<FalloAlEliminarSubastaException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

    }
}
