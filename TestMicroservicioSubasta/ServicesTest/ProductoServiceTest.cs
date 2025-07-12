using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Service;
using Domain.Entities;
using Domain.Value_Object;
using Moq.Protected;
using Moq;

namespace TestMicroservicioSubasta.Services
{
    public class ProductoServiceTest
    {
        private readonly Mock<HttpMessageHandler> _handlerMock = new();
        private readonly HttpClient _httpClient;
        private readonly ProductoService _service;

        public ProductoServiceTest()
        {
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5002")
            };
            _service = new ProductoService(_httpClient);
        }

        [Fact]
        public async Task ObtenerProductoPorGuid_DeberiaRetornarProducto_CuandoRespuestaExitosa()
        {
            var idProducto = Guid.NewGuid();
            var dto = new ProductoDTO
            {
                Id = idProducto,
                NombreProducto = "Laptop",
                DescripcionProducto = "16GB RAM",
                ImagenURLProducto = "https://img.com/laptop.jpg",
                PrecioBaseProducto = 3000,
                CategoriaProducto = "Tecnología",
                EstadoProducto = "Disponible"
            };

            var contenidoJson = JsonSerializer.Serialize(dto);
            SetupHttpResponse($"/api/Productos/consultarProducto/{idProducto}", HttpStatusCode.OK, contenidoJson);

            var result = await _service.ObtenerProductoPorGuid(idProducto);

            Assert.NotNull(result);
            Assert.Equal("Laptop", result.NombreProducto.Nombre);
            Assert.Equal("16GB RAM", result.DescripcionProducto.descripcion);
            Assert.Equal("Disponible", result.EstadoProducto.estadoProducto);
        }

        [Fact]
        public async Task ObtenerUsuarioIdPorIdProductoAsync_DeberiaRetornarGuid_CuandoRespuestaExitosa()
        {
            var idProducto = Guid.NewGuid();
            var idUsuario = Guid.NewGuid();
            SetupHttpResponse($"/api/Productos/consultarIdUsuarioProducto/{idProducto}", HttpStatusCode.OK, $"\"{idUsuario}\"");

            var result = await _service.ObtenerUsuarioIdPorIdProductoAsync(idProducto);

            Assert.Equal(idUsuario, result);
        }

        [Fact]
        public async Task ModificarProductoAsync_DeberiaRetornarTrue_CuandoPutExitosa()
        {
            var producto = new Producto(
                Guid.NewGuid(),
                new NombreProductoVO("Tablet"),
                new DescripcionProductoVO("Android"),
                new ImagenURLProductoVO("https://img.com/tablet.jpg"),
                new PrecioBaseProductoVO(1500),
                new CategoriaProductoVO("Tecnología"),
                new EstadoProductoVO("Disponible")
            );

            var uri = $"/api/Productos/modificarProducto/test@correo.com";
            SetupHttpResponse(uri, HttpStatusCode.OK, "");

            var result = await _service.ModificarProductoAsync("test@correo.com", producto, "Subastando");

            Assert.True(result);
        }

        [Fact]
        public async Task ModificarProductoDisponibleAsync_DeberiaRetornarFalse_CuandoPutFalla()
        {
            var producto = new Producto(
                Guid.NewGuid(),
                new NombreProductoVO("Tablet"),
                new DescripcionProductoVO("Android"),
                new ImagenURLProductoVO("https://img.com/tablet.jpg"),
                new PrecioBaseProductoVO(1500),
                new CategoriaProductoVO("Tecnología"),
                new EstadoProductoVO("Subastando")
            );

            var uri = $"/api/Productos/modificarProducto/test@correo.com";
            SetupHttpResponse(uri, HttpStatusCode.BadRequest, "");

            var result = await _service.ModificarProductoDisponibleAsync("test@correo.com", producto);

            Assert.False(result);
        }
        [Fact]
        public async Task ObtenerProductoPorGuid_DeberiaRetornarNull_CuandoRespuestaNoEsExitosa()
        {
            var idProducto = Guid.NewGuid();
            SetupHttpResponse($"/api/Productos/consultarProducto/{idProducto}", HttpStatusCode.NotFound, "");

            var result = await _service.ObtenerProductoPorGuid(idProducto);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObtenerProductoPorGuid_DeberiaRetornarNull_CuandoContenidoEsInvalido()
        {
            var idProducto = Guid.NewGuid();
            SetupHttpResponse($"/api/Productos/consultarProducto/{idProducto}", HttpStatusCode.OK, "contenido no JSON");

            var result = await _service.ObtenerProductoPorGuid(idProducto);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObtenerProductoPorGuid_DeberiaRetornarNull_CuandoExcepcionEsLanzada()
        {
            var idProducto = Guid.NewGuid();

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.RequestUri!.AbsolutePath.Contains($"/api/Productos/consultarProducto/{idProducto}")),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error de red"));

            var result = await _service.ObtenerProductoPorGuid(idProducto);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObtenerUsuarioIdPorIdProductoAsync_DeberiaRetornarGuidEmpty_CuandoRespuestaFalla()
        {
            var idProducto = Guid.NewGuid();
            SetupHttpResponse($"/api/Productos/consultarIdUsuarioProducto/{idProducto}", HttpStatusCode.InternalServerError, "");

            var result = await _service.ObtenerUsuarioIdPorIdProductoAsync(idProducto);

            Assert.Equal(Guid.Empty, result);
        }

        [Fact]
        public async Task ObtenerUsuarioIdPorIdProductoAsync_DeberiaRetornarGuidEmpty_CuandoNoEsGuidValido()
        {
            var idProducto = Guid.NewGuid();
            SetupHttpResponse($"/api/Productos/consultarIdUsuarioProducto/{idProducto}", HttpStatusCode.OK, "\"no-es-guid\"");

            var result = await _service.ObtenerUsuarioIdPorIdProductoAsync(idProducto);

            Assert.Equal(Guid.Empty, result);
        }

        [Fact]
        public async Task ModificarProductoAsync_DeberiaRetornarFalse_CuandoPutFalla()
        {
            var producto = new Producto(
                Guid.NewGuid(),
                new NombreProductoVO("Item"),
                new DescripcionProductoVO("Desc"),
                new ImagenURLProductoVO("https://img.com/x.jpg"),
                new PrecioBaseProductoVO(1200),
                new CategoriaProductoVO("Otros"),
                new EstadoProductoVO("Activo")
            );

            var uri = $"/api/Productos/modificarProducto/test@correo.com";
            SetupHttpResponse(uri, HttpStatusCode.BadRequest, "");

            var result = await _service.ModificarProductoAsync("test@correo.com", producto, "Cancelado");

            Assert.False(result);
        }


        private void SetupHttpResponse(string path, HttpStatusCode statusCode, string content)
        {
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.RequestUri!.AbsolutePath.Contains(path)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                });
        }

    }
}
