using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Handler;
using Application.Querys;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;
using Domain.Value_Objects;
using Moq;

namespace TestMicroservicioSubasta.HandlerTest
{
    public class ConsultarSubastasGanadasHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();
        private readonly Mock<IPujaService> _pujaServiceMock = new();

        private readonly ConsultarSubastasGanadasHandler _handler;

        public ConsultarSubastasGanadasHandlerTest()
        {
            _handler = new ConsultarSubastasGanadasHandler(
                _subastaServiceMock.Object,
                _usuarioServiceMock.Object,
                _productoServiceMock.Object,
                _pujaServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaDTO_CuandoExisteSubastaGanada()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var subasta = new Subasta(
                subastaId,
                new NombreSubastaVO("Subasta Inteligente"),
                new DescripcionSubastaVO("Dispositivos IoT"),
                productoId,
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-7)),
                new FechaFinSubastaVO(DateTime.UtcNow),
                new IncrementoMinimoSubastaVO(100),
                new PrecioReservaSubastaVO(2000),
                new EstadoSubastaVO("Finalizada"),
                usuarioId
            );

            var producto = new Producto(
                productoId,
                new NombreProductoVO("Smart Watch"),
                new DescripcionProductoVO("Pantalla AMOLED"),
                new ImagenURLProductoVO("https://img.com/smartwatch.jpg"),
                new PrecioBaseProductoVO(1500),
                new CategoriaProductoVO("Wearables"),
                new EstadoProductoVO("Nuevo")
            );

            var puja = new Puja(
                Guid.NewGuid(),
                usuarioId,
                subastaId,
                new MontoPujaVO(2100),
                new MontoMaximoPujaVO(2500),
                new TipoPujaVO("Manual"),
                new MontoPredeterminadoPujaVO(100),
                new FechaPujaVO(DateTime.UtcNow.AddDays(-1))
            );

            var historial = new HistorialSubasta(
                Guid.NewGuid(),
                subastaId,
                usuarioId,
                new MontoFinalSubastaVO(2100)
            );

            _subastaServiceMock.Setup(x => x.ObtenerSubastasGanadasMongoAsync())
                .ReturnsAsync(new List<Subasta> { subasta });

            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId))
                .ReturnsAsync(producto);

            _pujaServiceMock.Setup(x => x.ObtenerPujaGanadoraPorIdSubasta(subastaId))
                .ReturnsAsync(puja);

            _subastaServiceMock.Setup(x => x.ObtenerHistorialSubastaMongoAsync(subastaId))
                .ReturnsAsync(historial);

            _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(usuarioId))
                .ReturnsAsync("ganador@correo.com");

            var result = await _handler.Handle(new ConsultarSubastasGanadasQuery(), CancellationToken.None);

            Assert.Single(result);
            var dto = result.First();
            Assert.Equal("Subasta Inteligente", dto.NombreSubasta);
            Assert.Equal("Smart Watch", dto.NombreProducto);
            Assert.Equal(2100, dto.montoGanador);
            Assert.Equal("ganador@correo.com", dto.correoUsuario);
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaVacia_CuandoNoHaySubastasGanadas()
        {
            _subastaServiceMock.Setup(x => x.ObtenerSubastasGanadasMongoAsync())
                .ReturnsAsync(new List<Subasta>());

            var result = await _handler.Handle(new ConsultarSubastasGanadasQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_CuandoServicioFalla()
        {
            _subastaServiceMock.Setup(x => x.ObtenerSubastasGanadasMongoAsync())
                .ThrowsAsync(new Exception("fallo interno"));

            await Assert.ThrowsAsync<FalloAlObtenerSubastasException>(() =>
                _handler.Handle(new ConsultarSubastasGanadasQuery(), CancellationToken.None));
        }

    }
}
