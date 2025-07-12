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
using Moq;

namespace TestMicroservicioSubasta.HandlerTest
{
    public class ConsultarSubastasHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();

        private readonly ConsultarSubastasHandler _handler;

        public ConsultarSubastasHandlerTest()
        {
            _handler = new ConsultarSubastasHandler(
                _subastaServiceMock.Object,
                _usuarioServiceMock.Object,
                _productoServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaDTO_CuandoSubastasExisten()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var subasta = new Subasta(
                subastaId,
                new NombreSubastaVO("Subasta Vintage"),
                new DescripcionSubastaVO("Colección de antigüedades"),
                productoId,
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-10)),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(2)),
                new IncrementoMinimoSubastaVO(50),
                new PrecioReservaSubastaVO(1500),
                new EstadoSubastaVO("Activa"),
                usuarioId
            );

            var producto = new Producto(
                productoId,
                new NombreProductoVO("Radio Antiguo"),
                new DescripcionProductoVO("Fabricado en 1940"),
                new ImagenURLProductoVO("https://img.com/radio.jpg"),
                new PrecioBaseProductoVO(1200),
                new CategoriaProductoVO("Coleccionismo"),
                new EstadoProductoVO("Conservado")
            );

            _subastaServiceMock.Setup(x => x.ObtenerSubastasMongo())
                .ReturnsAsync(new List<Subasta> { subasta });

            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId))
                .ReturnsAsync(producto);

            var result = await _handler.Handle(new ConsultarSubastasQuery(), CancellationToken.None);

            Assert.Single(result);
            var dto = result.First();
            Assert.Equal("Subasta Vintage", dto.NombreSubasta);
            Assert.Equal("Radio Antiguo", dto.NombreProducto);
            Assert.Equal("Coleccionismo", dto.Categoria);
            Assert.Equal("https://img.com/radio.jpg", dto.urlImagen);
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaVacia_CuandoNoHaySubastas()
        {
            _subastaServiceMock.Setup(x => x.ObtenerSubastasMongo())
                .ReturnsAsync(new List<Subasta>());

            var result = await _handler.Handle(new ConsultarSubastasQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_CuandoServicioFalla()
        {
            _subastaServiceMock.Setup(x => x.ObtenerSubastasMongo())
                .ThrowsAsync(new Exception("Falla inesperada"));

            await Assert.ThrowsAsync<FalloAlObtenerSubastasException>(() =>
                _handler.Handle(new ConsultarSubastasQuery(), CancellationToken.None));
        }

    }
}
