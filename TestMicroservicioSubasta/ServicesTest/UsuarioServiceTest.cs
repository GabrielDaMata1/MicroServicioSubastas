using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq.Protected;
using Moq;

namespace TestMicroservicioSubasta.ServicesTest
{
    public class UsuarioServiceTest
    {
        private const string BaseUrl = "http://localhost:5001/";
        private const string EndpointIdUsuario = "api/usuarios/IdUsuario/";
        private const string EndpointCorreo = "api/usuarios/Correo/";

        private HttpClient CrearHttpClient(Mock<HttpMessageHandler> handlerMock)
        {
            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        [Fact]
        public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarGuid_CuandoRespuestaEsExitosa()
        {
            var expectedGuid = Guid.NewGuid();
            var correo = "usuario@ejemplo.com";
            var expectedUrl = $"{BaseUrl}{EndpointIdUsuario}{correo}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"\"{expectedGuid}\"")
                });

            var httpClient = CrearHttpClient(handlerMock);
            var service = new UsuarioService(httpClient);

            var resultado = await service.ObtenerUsuarioPorIdAsync(correo);

            Assert.Equal(expectedGuid, resultado);
        }

        [Fact]
        public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarGuidEmpty_CuandoRespuestaEsFallida()
        {
            var correo = "usuario@ejemplo.com";
            var expectedUrl = $"{BaseUrl}{EndpointIdUsuario}{correo}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = CrearHttpClient(handlerMock);
            var service = new UsuarioService(httpClient);

            var resultado = await service.ObtenerUsuarioPorIdAsync(correo);

            Assert.Equal(Guid.Empty, resultado);
        }

        [Fact]
        public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarGuidEmpty_CuandoContenidoNoEsGuid()
        {
            var correo = "usuario@ejemplo.com";
            var expectedUrl = $"{BaseUrl}{EndpointIdUsuario}{correo}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("\"no-es-un-guid\"")
                });

            var httpClient = CrearHttpClient(handlerMock);
            var service = new UsuarioService(httpClient);

            var resultado = await service.ObtenerUsuarioPorIdAsync(correo);

            Assert.Equal(Guid.Empty, resultado);
        }

        [Fact]
        public async Task ObtenerCorreoPorIdAsync_DeberiaRetornarCorreo_CuandoRespuestaEsExitosa()
        {
            var idUsuario = Guid.NewGuid();
            var correoEsperado = "usuario@ejemplo.com";
            var expectedUrl = $"{BaseUrl}{EndpointCorreo}{idUsuario}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(correoEsperado)
                });

            var httpClient = CrearHttpClient(handlerMock);
            var service = new UsuarioService(httpClient);

            var resultado = await service.ObtenerCorreoPorIdAsync(idUsuario);

            Assert.Equal(correoEsperado, resultado);
        }

        [Fact]
        public async Task ObtenerCorreoPorIdAsync_DeberiaRetornarNull_CuandoRespuestaEsFallida()
        {
            var idUsuario = Guid.NewGuid();
            var expectedUrl = $"{BaseUrl}{EndpointCorreo}{idUsuario}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = CrearHttpClient(handlerMock);
            var service = new UsuarioService(httpClient);

            var resultado = await service.ObtenerCorreoPorIdAsync(idUsuario);

            Assert.Null(resultado);
        }

    }
}
