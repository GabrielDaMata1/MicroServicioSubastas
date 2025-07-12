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
using Domain.Factory;
using Domain.Interfaces;
using Domain.Value_Object;
using MassTransit;
using Moq;

namespace TestMicroservicioSubasta.HandlerTest
{
    public class RegistrarSubastaHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();
        private readonly Mock<ISubastaSchedule> _subastaScheduleMock = new();

        private readonly RegistrarSubastaHandler _handler;

        public RegistrarSubastaHandlerTest()
        {
            _handler = new RegistrarSubastaHandler(
                _subastaServiceMock.Object,
                _publishEndpointMock.Object,
                _usuarioServiceMock.Object,
                _productoServiceMock.Object,
                _subastaScheduleMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarTrue_CuandoRegistroExitosa()
        {
            var correo = "usuario@correo.com";
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var subasta = SubastaFactory.CrearSubasta("Subasta Arte", "Obras clásicas", productoId, DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(3), 100, 2000, "Pending");
            var producto = new Producto(productoId, new NombreProductoVO("Cuadro"), new DescripcionProductoVO("Réplica"),
                new ImagenURLProductoVO("https://img.com/art.jpg"), new PrecioBaseProductoVO(1800),
                new CategoriaProductoVO("Arte"), new EstadoProductoVO("Disponible"));

            var command = new RegistrarSubastaCommand(new RegistrarSubastaDTO
            {
                Nombre = "Subasta Arte",
                Descripcion = "Obras clásicas",
                idProducto = productoId,
                fechaInicio = DateTime.UtcNow.AddDays(1),
                fechaFin = DateTime.UtcNow.AddDays(3),
                incrementoMinimo = 100,
                precioReserva = 2000,
                correoUsuario = correo
            });

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId)).ReturnsAsync(usuarioId);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);
            _subastaServiceMock.Setup(x => x.RegistrarSubastaPostgreSQLAsync(It.IsAny<Subasta>(), usuarioId))
                .ReturnsAsync(Guid.NewGuid());
            _productoServiceMock.Setup(x => x.ModificarProductoAsync(correo, producto, "Subastando"))
                .ReturnsAsync(true);
            _subastaScheduleMock.Setup(x => x.ProgramarEventosDeSubasta(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<SubastaRegistradaEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarProductoNoPerteneceAlUsuarioException_CuandoIdsNoCoinciden()
        {
            var command = new RegistrarSubastaCommand(new RegistrarSubastaDTO
            {
                correoUsuario = "usuario@correo.com",
                idProducto = Guid.NewGuid()
            });

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(command.SubastaDto.correoUsuario))
                .ReturnsAsync(Guid.NewGuid());

            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(command.SubastaDto.idProducto))
                .ReturnsAsync(Guid.NewGuid());

            await Assert.ThrowsAsync<ProductoNoPerteneceAlUsuarioException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarProductoYaSubastandoException_CuandoEstadoEsSubastando()
        {
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var command = new RegistrarSubastaCommand(new RegistrarSubastaDTO
            {
                correoUsuario = "usuario@correo.com",
                idProducto = productoId
            });

            var producto = new Producto(
                productoId,
                new NombreProductoVO("Item"),
                new DescripcionProductoVO("Detalle"),
                new ImagenURLProductoVO("url"),
                new PrecioBaseProductoVO(100),
                new CategoriaProductoVO("Genérico"),
                new EstadoProductoVO("Subastando")
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(command.SubastaDto.correoUsuario))
                .ReturnsAsync(usuarioId);

            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId))
                .ReturnsAsync(usuarioId);

            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId))
                .ReturnsAsync(producto);

            await Assert.ThrowsAsync<ProductoYaSubastandoException>(() =>
                _handler.Handle(command, CancellationToken.None));


        }

        [Fact]
        public async Task Handle_DeberiaLanzarFalloAlRegistrarSubastaException_CuandoRegistroFalla()
        {
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var correo = "usuario@correo.com";

            var command = new RegistrarSubastaCommand(new RegistrarSubastaDTO
            {
                Nombre = "Subasta",
                Descripcion = "Prueba",
                idProducto = productoId,
                fechaInicio = DateTime.UtcNow.AddDays(1),
                fechaFin = DateTime.UtcNow.AddDays(3),
                incrementoMinimo = 50,
                precioReserva = 1000,
                correoUsuario = correo
            });

            var producto = new Producto(productoId, new NombreProductoVO("Item"), new DescripcionProductoVO("Info"),
                new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(999),
                new CategoriaProductoVO("Otros"), new EstadoProductoVO("Disponible"));

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId)).ReturnsAsync(usuarioId);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);
            _subastaServiceMock.Setup(x => x.RegistrarSubastaPostgreSQLAsync(It.IsAny<Subasta>(), usuarioId)).ReturnsAsync(Guid.Empty);

            await Assert.ThrowsAsync<FalloAlRegistrarSubastaException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarFalloAlModificarProductoException_CuandoModificacionFalla()
        {
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var correo = "usuario@correo.com";

            var command = new RegistrarSubastaCommand(new RegistrarSubastaDTO
            {
                Nombre = "Subasta",
                Descripcion = "Prueba",
                idProducto = productoId,
                fechaInicio = DateTime.UtcNow.AddDays(1),
                fechaFin = DateTime.UtcNow.AddDays(2),
                incrementoMinimo = 30,
                precioReserva = 900,
                correoUsuario = correo
            });

            var producto = new Producto(productoId, new NombreProductoVO("Item"), new DescripcionProductoVO("Info"),
                new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(850),
                new CategoriaProductoVO("Otros"), new EstadoProductoVO("Disponible"));

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId)).ReturnsAsync(usuarioId);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);
            _subastaServiceMock.Setup(x => x.RegistrarSubastaPostgreSQLAsync(It.IsAny<Subasta>(), usuarioId)).ReturnsAsync(Guid.NewGuid());
            _productoServiceMock.Setup(x => x.ModificarProductoAsync(correo, producto, "Subastando")).ReturnsAsync(false);

            await Assert.ThrowsAsync<FalloAlModificarProductoException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

    }
}
