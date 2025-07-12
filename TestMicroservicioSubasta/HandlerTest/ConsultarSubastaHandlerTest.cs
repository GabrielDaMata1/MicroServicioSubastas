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
    public class ConsultarSubastaHandlerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();

        private readonly ConsultarSubastaHandler _handler;

        public ConsultarSubastaHandlerTest()
        {
            _handler = new ConsultarSubastaHandler(
                _subastaServiceMock.Object,
                _usuarioServiceMock.Object,
                _productoServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaRetornarDTOCompleto_CuandoSubastaExiste()
        {
            var idSubasta = Guid.NewGuid();
            var idProducto = Guid.NewGuid();

            var query = new ConsultarSubastaQuery(new ConsultarSubastaDTO { idSubasta = idSubasta });

            var subasta = new Subasta(
                idSubasta,
                new NombreSubastaVO("Subasta Gamer"),
                new DescripcionSubastaVO("Consolas y periféricos"),
                idProducto,
                new FechaInicioSubastaVO(DateTime.UtcNow),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(3)),
                new IncrementoMinimoSubastaVO(100),
                new PrecioReservaSubastaVO(3000),
                new EstadoSubastaVO("Activa"),
                Guid.NewGuid()
            );

            var producto = new Producto(
                idProducto,
                new NombreProductoVO("PlayStation 5"),
                new DescripcionProductoVO("Edición limitada"),
                new ImagenURLProductoVO("https://img.com/ps5.jpg"),
                new PrecioBaseProductoVO(2500),
                new CategoriaProductoVO("Tecnología"),
                new EstadoProductoVO("Disponible")
            );


            _subastaServiceMock.Setup(x => x.ObtenerSubastaMongoAsync(idSubasta)).ReturnsAsync(subasta);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(idProducto)).ReturnsAsync(producto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(idSubasta, result.IdSubasta);
            Assert.Equal("Subasta Gamer", result.NombreSubasta);
            Assert.Equal("Consolas y periféricos", result.DescripcionSubasta);
            Assert.Equal("Activa", result.Estado);
            Assert.Equal("PlayStation 5", result.NombreProducto);
            Assert.Equal("Edición limitada", result.DescripcionProducto);
            Assert.Equal("Tecnología", result.Categoria);
            Assert.Equal("https://img.com/ps5.jpg", result.urlImagen);
        }

        [Fact]
        public async Task Handle_DeberiaRetornarDTOVacio_CuandoSubastaEsNull()
        {
            var query = new ConsultarSubastaQuery(new ConsultarSubastaDTO { idSubasta = Guid.NewGuid() });

            _subastaServiceMock.Setup(x => x.ObtenerSubastaMongoAsync(query.SubastaDto.idSubasta)).ReturnsAsync((Subasta)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.IdSubasta);
            Assert.Null(result.NombreSubasta);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_CuandoServicioFalla()
        {
            var query = new ConsultarSubastaQuery(new ConsultarSubastaDTO { idSubasta = Guid.NewGuid() });

            _subastaServiceMock.Setup(x => x.ObtenerSubastaMongoAsync(query.SubastaDto.idSubasta))
                .ThrowsAsync(new Exception("Error inesperado"));

            await Assert.ThrowsAsync<FalloAlObtenerSubastasException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

    }
}
