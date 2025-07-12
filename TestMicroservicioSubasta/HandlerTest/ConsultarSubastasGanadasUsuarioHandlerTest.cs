using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
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
    public class ConsultarSubastasGanadasUsuarioHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();
        private readonly Mock<IPujaService> _pujaServiceMock = new();

        private readonly ConsultarSubastasGanadasUsuarioHandler _handler;

        public ConsultarSubastasGanadasUsuarioHandlerTest()
        {
            _handler = new ConsultarSubastasGanadasUsuarioHandler(
                _subastaServiceMock.Object,
                _usuarioServiceMock.Object,
                _productoServiceMock.Object,
                _pujaServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaDTO_CuandoSubastasGanadasExisten()
        {
            var correo = "usuario@correo.com";
            var usuarioId = Guid.NewGuid();
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();

            var query = new ConsultarSubastasGanadasUsuarioQuery(
                new ConsultarSubastasUsuarioDTO { correoUsuario = correo }
            );

            var subasta = new Subasta(
                subastaId,
                new NombreSubastaVO("Subasta Legendaria"),
                new DescripcionSubastaVO("Arte y reliquias"),
                productoId,
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-10)),
                new FechaFinSubastaVO(DateTime.UtcNow),
                new IncrementoMinimoSubastaVO(100),
                new PrecioReservaSubastaVO(5000),
                new EstadoSubastaVO("Finalizada"),
                usuarioId
            );

            var producto = new Producto(
                productoId,
                new NombreProductoVO("Pintura Original"),
                new DescripcionProductoVO("Siglo XVII"),
                new ImagenURLProductoVO("https://img.com/arte.jpg"),
                new PrecioBaseProductoVO(4000),
                new CategoriaProductoVO("Arte"),
                new EstadoProductoVO("Auténtico")
            );

            var pujaGanadora = new Puja(
                Guid.NewGuid(),
                usuarioId,
                subastaId,
                new MontoPujaVO(5100),
                new MontoMaximoPujaVO(5500),
                new TipoPujaVO("Manual"),
                new MontoPredeterminadoPujaVO(200),
                new FechaPujaVO(DateTime.UtcNow.AddDays(-1))
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.ObtenerSubastasGanadasDetalleMongoAsync(usuarioId))
                .ReturnsAsync(new List<Subasta> { subasta });
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);
            _pujaServiceMock.Setup(x => x.ObtenerPujaGanadoraPorIdSubasta(subastaId)).ReturnsAsync(pujaGanadora);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            var dto = result.First();
            Assert.Equal("Subasta Legendaria", dto.NombreSubasta);
            Assert.Equal("Pintura Original", dto.NombreProducto);
            Assert.Equal(5100, dto.montoGanador);
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaVacia_CuandoNoHaySubastasGanadas()
        {
            var correo = "usuario@vacio.com";
            var usuarioId = Guid.NewGuid();

            var query = new ConsultarSubastasGanadasUsuarioQuery(
                new ConsultarSubastasUsuarioDTO { correoUsuario = correo }
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.ObtenerSubastasGanadasDetalleMongoAsync(usuarioId))
                .ReturnsAsync(new List<Subasta>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_CuandoServicioFalla()
        {
            var query = new ConsultarSubastasGanadasUsuarioQuery(
                new ConsultarSubastasUsuarioDTO { correoUsuario = "error@correo.com" }
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Falla"));

            await Assert.ThrowsAsync<FalloAlObtenerSubastasException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

    }
}
