using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Events;
using Domain.Interfaces;
using Infrastructure.Consumers;
using MassTransit;
using Moq;

namespace TestMicroservicioSubasta.ConsumersTest
{
    public class EntregaPremioConfirmadaConsumerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly EntregaPremioConfirmadaConsumer _consumer;

        public EntregaPremioConfirmadaConsumerTest()
        {
            _consumer = new EntregaPremioConfirmadaConsumer(_subastaServiceMock.Object);
        }

        [Fact]
        public async Task Consume_DeberiaActualizarEstados_CuandoPostgreSQLRetornaOK()
        {
            var subastaId = Guid.NewGuid();
            var mensaje = new EntregaPremioConfirmadaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<EntregaPremioConfirmadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(mensaje);

            _subastaServiceMock
                .Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Delivered"))
                .ReturnsAsync(HttpStatusCode.OK);

            _subastaServiceMock
                .Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Delivered"))
                .ReturnsAsync(HttpStatusCode.OK);

            await _consumer.Consume(contextoMock.Object);

            _subastaServiceMock.Verify(x =>
                x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Delivered"), Times.Once);

            _subastaServiceMock.Verify(x =>
                x.ActualizarEstadoSubastaMongoAsync(subastaId, "Delivered"), Times.Once);
        }

        [Fact]
        public async Task Consume_NoDeberiaActualizarMongo_CuandoPostgreSQLRetornaError()
        {
            var subastaId = Guid.NewGuid();
            var mensaje = new EntregaPremioConfirmadaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<EntregaPremioConfirmadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(mensaje);

            _subastaServiceMock
                .Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Delivered"))
                .ReturnsAsync(HttpStatusCode.BadRequest);

            await _consumer.Consume(contextoMock.Object);

            _subastaServiceMock.Verify(x =>
                x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Delivered"), Times.Once);

            _subastaServiceMock.Verify(x =>
                x.ActualizarEstadoSubastaMongoAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarExcepcion_CuandoServicioFalla()
        {
            var subastaId = Guid.NewGuid();
            var mensaje = new EntregaPremioConfirmadaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<EntregaPremioConfirmadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(mensaje);

            _subastaServiceMock
                .Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Delivered"))
                .ThrowsAsync(new Exception("Fallo interno"));

            await Assert.ThrowsAsync<FalloAlModificarSubastaException>(() =>
                _consumer.Consume(contextoMock.Object));
        }

    }
}
