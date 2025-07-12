using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Service;
using Domain.Interfaces;
using Moq.Protected;
using Moq;

namespace TestMicroservicioSubasta.Services
{
    public class PujaServiceTest
    {
        private readonly Mock<HttpMessageHandler> _handlerMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly HttpClient _httpClient;
        private readonly PujaService _service;

        public PujaServiceTest()
        {
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5004")
            };
            _service = new PujaService(_httpClient, _usuarioServiceMock.Object);
        }

        [Fact]
        public async Task ObtenerPujaGanadoraPorIdSubasta_DeberiaRetornarPuja_CuandoRespuestaExitosa()
        {
            var subastaId = Guid.NewGuid();
            var dto = new PujaDTO
            {
                id = Guid.NewGuid(),
                idUsuario = Guid.NewGuid(),
                idSubasta = subastaId,
                montoPuja = 3000,
                montoMaximo = 3500,
                tipoPuja = "Manual",
                montoPredeterminado = 200
            };

            var contenidoJson = JsonSerializer.Serialize(dto);
            SetupHttpResponse($"/api/Pujas/obtenerPujaGanadora/{subastaId}", HttpStatusCode.OK, contenidoJson);

            var result = await _service.ObtenerPujaGanadoraPorIdSubasta(subastaId);

            Assert.NotNull(result);
            Assert.Equal(3000, result.MontoPuja.montoPuja);
            Assert.Equal("Manual", result.TipoPuja.tipoPuja);
        }

        [Fact]
        public async Task ObtenerPujasSubasta_DeberiaRetornarLista_CuandoRespuestaExitosa()
        {
            var subastaId = Guid.NewGuid();
            var correoUsuario = "test@correo.com";
            var idUsuarioSimulado = Guid.NewGuid();

            var dto = new List<PujaUsuarioDTO>
        {
            new PujaUsuarioDTO
            {
                id = Guid.NewGuid(),
                idSubasta = subastaId,
                correoUsuario = correoUsuario,
                montoPuja = 2500,
                montoMaximo = 3000,
                tipoPuja = "Automática",
                montoPredeterminado = 100,
                fecha = DateTime.UtcNow
            }
        };

            var contenidoJson = JsonSerializer.Serialize(dto);
            SetupHttpResponse($"/api/Pujas/obtenerPujasSubasta/{subastaId}", HttpStatusCode.OK, contenidoJson);

            _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correoUsuario)).ReturnsAsync(idUsuarioSimulado);

            var result = await _service.ObtenerPujasSubasta(subastaId);

            Assert.NotNull(result);
            Assert.Single(result);
            var puja = result.First();
            Assert.Equal(2500, puja.MontoPuja.montoPuja);
            Assert.Equal("Automática", puja.TipoPuja.tipoPuja);
            Assert.Equal(idUsuarioSimulado, puja.IdUsuario);
        }
        [Fact]
        public async Task ObtenerPujaGanadoraPorIdSubasta_DeberiaRetornarNull_CuandoRespuestaNoExitosa()
        {
            var subastaId = Guid.NewGuid();
            SetupHttpResponse($"/api/Pujas/obtenerPujaGanadora/{subastaId}", HttpStatusCode.NotFound, "");

            var result = await _service.ObtenerPujaGanadoraPorIdSubasta(subastaId);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObtenerPujaGanadoraPorIdSubasta_DeberiaRetornarNull_CuandoContenidoInvalido()
        {
            var subastaId = Guid.NewGuid();
            SetupHttpResponse($"/api/Pujas/obtenerPujaGanadora/{subastaId}", HttpStatusCode.OK, "contenido corrupto");

            var result = await _service.ObtenerPujaGanadoraPorIdSubasta(subastaId);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObtenerPujaGanadoraPorIdSubasta_DeberiaRetornarNull_CuandoSeLanzaExcepcion()
        {
            var subastaId = Guid.NewGuid();

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsolutePath.Contains($"/api/Pujas/obtenerPujaGanadora/{subastaId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error de red"));

            var result = await _service.ObtenerPujaGanadoraPorIdSubasta(subastaId);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObtenerPujasSubasta_DeberiaRetornarNull_CuandoRespuestaNoExitosa()
        {
            var subastaId = Guid.NewGuid();
            SetupHttpResponse($"/api/Pujas/obtenerPujasSubasta/{subastaId}", HttpStatusCode.BadRequest, "");

            var result = await _service.ObtenerPujasSubasta(subastaId);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObtenerPujasSubasta_DeberiaRetornarNull_CuandoContenidoInvalido()
        {
            var subastaId = Guid.NewGuid();
            SetupHttpResponse($"/api/Pujas/obtenerPujasSubasta/{subastaId}", HttpStatusCode.OK, "no json válido");

            var result = await _service.ObtenerPujasSubasta(subastaId);

            Assert.Null(result);
        }

        [Fact]
        public async Task ObtenerPujasSubasta_DeberiaRetornarNull_CuandoSeLanzaExcepcion()
        {
            var subastaId = Guid.NewGuid();

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsolutePath.Contains($"/api/Pujas/obtenerPujasSubasta/{subastaId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error simulado"));

            var result = await _service.ObtenerPujasSubasta(subastaId);

            Assert.Null(result);
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
