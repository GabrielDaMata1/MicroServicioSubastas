using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Service;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace TestMicroservicioSubasta.Services
{
    public class NotificacionServiceTest
    {
        private readonly Mock<HttpMessageHandler> _httpHandlerMock = new();
        private readonly HttpClient _httpClient;
        private readonly NotificacionService _service;

        public NotificacionServiceTest()
        {
            _httpClient = new HttpClient(_httpHandlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5287")
            };
            var configMock = new Mock<IConfiguration>();
            _service = new NotificacionService(_httpClient, configMock.Object);
        }

        [Fact]
        public async Task EnviarNotificacionUsuarioGanadorSubasta_DeberiaRetornarTrue_CuandoRespuestaExitosa()
        {
            SetupResponse("/api/Notification/enviarNotificacionUsuarioGanadorSubasta", HttpStatusCode.OK);

            var resultado = await _service.EnviarNotificacionUsuarioGanadorSubasta("ganador@correo.com", 2500, "Subasta Oro", "Anillo");

            Assert.True(resultado);
        }

        [Fact]
        public async Task EnviarNotificacionSubastadorSubastaFinalizada_DeberiaRetornarFalse_CuandoRespuestaFalla()
        {
            SetupResponse("/api/Notification/enviarNotificacionSubastadorSubastaFinalizada", HttpStatusCode.BadRequest);

            var resultado = await _service.EnviarNotificacionSubastadorSubastaFinalizada("subastador@correo.com", 2500, "Subasta VIP", "Reloj", "comprador@correo.com");

            Assert.False(resultado);
        }

        [Fact]
        public async Task EnviarCorreoUsuariosSubastaCancelada_DeberiaRetornarTrue_CuandoRespuestaExitosa()
        {
            SetupResponse("/api/Notification/enviarCorreoUsuariosSubastaCancelada", HttpStatusCode.OK);

            var resultado = await _service.EnviarCorreoUsuariosSubastaCancelada("subastador@correo.com", "usuario@correo.com", "Subasta Cancelada", "Smartwatch");

            Assert.True(resultado);
        }

        private void SetupResponse(string path, HttpStatusCode statusCode)
        {
            _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(msg => msg.RequestUri!.AbsolutePath.Contains(path)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent("{}", Encoding.UTF8, "application/json")
                });
        }
        [Fact]
        public async Task EnviarNotificacionSubastadorSubastaFinalizada_DeberiaRetornarFalse_CuandoSeLanzaExcepcionHttp()
        {
            var path = "/api/Notification/enviarNotificacionSubastadorSubastaFinalizada";

            _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(msg => msg.RequestUri!.AbsolutePath.Contains(path)),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Fallo de conexión simulada"));

            var resultado = await _service.EnviarNotificacionSubastadorSubastaFinalizada(
                "subastador@correo.com", 2500, "Subasta VIP", "Reloj", "comprador@correo.com");

            Assert.False(resultado);
        }

        [Fact]
        public async Task EnviarNotificacionUsuarioGanadorSubasta_DeberiaRetornarFalse_CuandoSeLanzaExcepcion()
        {
            var path = "/api/Notification/enviarNotificacionUsuarioGanadorSubasta";

            _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsolutePath.Contains(path)),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error simulado de red"));

            var resultado = await _service.EnviarNotificacionUsuarioGanadorSubasta(
                "ganador@correo.com", 3500, "Subasta VIP", "Collar de perlas");

            Assert.False(resultado);
        }

    }
}
