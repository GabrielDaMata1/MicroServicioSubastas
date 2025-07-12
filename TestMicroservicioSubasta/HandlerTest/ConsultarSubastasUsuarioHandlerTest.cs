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
using Moq;

namespace TestMicroservicioSubasta.HandlerTest
{
    public class ConsultarSubastasUsuarioHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();

        private readonly ConsultarSubastasUsuarioHandler _handler;

        public ConsultarSubastasUsuarioHandlerTest()
        {
            _handler = new ConsultarSubastasUsuarioHandler(
                _subastaServiceMock.Object,
                _usuarioServiceMock.Object,
                _productoServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaDTO_CuandoExistenSubastas()
        {
            var correo = "subastador@correo.com";
            var usuarioId = Guid.NewGuid();
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();

            var query = new ConsultarSubastasUsuarioQuery(
                new ConsultarSubastasUsuarioDTO { correoUsuario = correo }
            );

            var subasta = new Subasta(
                subastaId,
                new NombreSubastaVO("Subasta Retro"),
                new DescripcionSubastaVO("Objetos de colección"),
                productoId,
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-5)),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(2)),
                new IncrementoMinimoSubastaVO(150),
                new PrecioReservaSubastaVO(3000),
                new EstadoSubastaVO("Activa"),
                usuarioId
            );

            var producto = new Producto(
                productoId,
                new NombreProductoVO("Walkman Sony"),
                new DescripcionProductoVO("Año 1985"),
                new ImagenURLProductoVO("https://img.com/walkman.jpg"),
                new PrecioBaseProductoVO(2000),
                new CategoriaProductoVO("Electrónica"),
                new EstadoProductoVO("Nuevo")
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.ObtenerSubastasPorUsuarioMongoAsync(usuarioId)).ReturnsAsync(new List<Subasta> { subasta });
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            var dto = result.First();
            Assert.Equal("Subasta Retro", dto.NombreSubasta);
            Assert.Equal("Walkman Sony", dto.NombreProducto);
            Assert.Equal("Electrónica", dto.Categoria);
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaVacia_CuandoNoHaySubastas()
        {
            var correo = "vacio@correo.com";
            var usuarioId = Guid.NewGuid();

            var query = new ConsultarSubastasUsuarioQuery(
                new ConsultarSubastasUsuarioDTO { correoUsuario = correo }
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(usuarioId);
            _subastaServiceMock.Setup(x => x.ObtenerSubastasPorUsuarioMongoAsync(usuarioId)).ReturnsAsync(new List<Subasta>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_CuandoServicioFalla()
        {
            var query = new ConsultarSubastasUsuarioQuery(
                new ConsultarSubastasUsuarioDTO { correoUsuario = "error@correo.com" }
            );

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Falla interna"));

            await Assert.ThrowsAsync<FalloAlObtenerSubastasException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

    }
}
