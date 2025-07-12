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
    public class ConsultarSubastasGanadasPujasHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();
        private readonly Mock<IPujaService> _pujaServiceMock = new();

        private readonly ConsultarSubastasGanadasPujasHandler _handler;

        public ConsultarSubastasGanadasPujasHandlerTest()
        {
            _handler = new ConsultarSubastasGanadasPujasHandler(
                _subastaServiceMock.Object,
                _usuarioServiceMock.Object,
                _productoServiceMock.Object,
                _pujaServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaDTO_CuandoSubastasGanadasExisten()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var subasta = new Subasta(
                subastaId,
                new NombreSubastaVO("Subasta Premium"),
                new DescripcionSubastaVO("Artículos de colección"),
                productoId,
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-5)),
                new FechaFinSubastaVO(DateTime.UtcNow),
                new IncrementoMinimoSubastaVO(100),
                new PrecioReservaSubastaVO(5000),
                new EstadoSubastaVO("Finalizada"),
                usuarioId
            );

            var producto = new Producto(
                productoId,
                new NombreProductoVO("Reloj antiguo"),
                new DescripcionProductoVO("Modelo suizo 1890"),
                new ImagenURLProductoVO("https://img.com/reloj.jpg"),
                new PrecioBaseProductoVO(4500),
                new CategoriaProductoVO("Antigüedades"),
                new EstadoProductoVO("Nuevo")
            );

            var puja = new Puja(
                Guid.NewGuid(),
                usuarioId,
                subastaId,
                new MontoPujaVO(5200),
                new MontoMaximoPujaVO(5500),
                new TipoPujaVO("Manual"),
                new MontoPredeterminadoPujaVO(200),
                new FechaPujaVO(DateTime.UtcNow.AddDays(-2))
            );

            _subastaServiceMock.Setup(x => x.ObtenerSubastasGanadasMongoAsync())
                .ReturnsAsync(new List<Subasta> { subasta });

            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId))
                .ReturnsAsync(producto);

            _pujaServiceMock.Setup(x => x.ObtenerPujasSubasta(subastaId))
                .ReturnsAsync(new List<Puja> { puja });

            _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(usuarioId))
                .ReturnsAsync("ganador@correo.com");

            var result = await _handler.Handle(new ConsultarSubastasGanadasPujasQuery(), CancellationToken.None);

            Assert.Single(result);
            var dto = result.First();
            Assert.Equal("Subasta Premium", dto.NombreSubasta);
            Assert.Single(dto.Pujas);
            Assert.Equal("ganador@correo.com", dto.Pujas.First().correoUsuario);
            Assert.Equal(5200, dto.Pujas.First().montoPuja);
            Assert.Equal("Manual", dto.Pujas.First().tipoPuja);
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaVacia_CuandoNoHaySubastasGanadas()
        {
            _subastaServiceMock.Setup(x => x.ObtenerSubastasGanadasMongoAsync())
                .ReturnsAsync(new List<Subasta>());

            var result = await _handler.Handle(new ConsultarSubastasGanadasPujasQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarFalloAlObtenerSubastasException_CuandoServicioFalla()
        {
            _subastaServiceMock.Setup(x => x.ObtenerSubastasGanadasMongoAsync())
                .ThrowsAsync(new Exception("Error"));

            await Assert.ThrowsAsync<FalloAlObtenerSubastasException>(() =>
                _handler.Handle(new ConsultarSubastasGanadasPujasQuery(), CancellationToken.None));
        }

    }
}
