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
    public class SubastaActivaConsumerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly SubastaActivaConsumer _consumer;

        public SubastaActivaConsumerTest()
        {
            _consumer = new SubastaActivaConsumer(_subastaServiceMock.Object);
        }

        [Fact]
        public async Task Consume_ActualizaPostgreSQLYMongo_CuandoPostgreSQLRetornaOK()
        {
            var subastaId = Guid.NewGuid();
            var contexto = Mock.Of<ConsumeContext<SubastaActivaEvent>>(c => c.Message == new SubastaActivaEvent
            (
                subastaId
            ));

            _subastaServiceMock.Setup(x =>
                x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Active"))
                .ReturnsAsync(HttpStatusCode.OK);

            _subastaServiceMock.Setup(x =>
                x.ActualizarEstadoSubastaMongoAsync(subastaId, "Active"))
                .ReturnsAsync(HttpStatusCode.OK);

            await _consumer.Consume(contexto);

            _subastaServiceMock.Verify(x =>
                x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Active"), Times.Once);

            _subastaServiceMock.Verify(x =>
                x.ActualizarEstadoSubastaMongoAsync(subastaId, "Active"), Times.Once);
        }

        [Fact]
        public async Task Consume_NoActualizaMongo_CuandoPostgreSQLRetornaBadRequest()
        {
            var subastaId = Guid.NewGuid();
            var contexto = Mock.Of<ConsumeContext<SubastaActivaEvent>>(c => c.Message == new SubastaActivaEvent
            (
                subastaId
            ));

            _subastaServiceMock.Setup(x =>
                x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Active"))
                .ReturnsAsync(HttpStatusCode.BadRequest);

            await _consumer.Consume(contexto);

            _subastaServiceMock.Verify(x =>
                x.ActualizarEstadoSubastaMongoAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Consume_LanzaExcepcion_CuandoPostgreSQLFalla()
        {
            var subastaId = Guid.NewGuid();
            var contexto = Mock.Of<ConsumeContext<SubastaActivaEvent>>(c => c.Message == new SubastaActivaEvent
            (
                subastaId
            ));

            _subastaServiceMock.Setup(x =>
                x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Active"))
                .ThrowsAsync(new Exception("error"));

            await Assert.ThrowsAsync<FalloAlModificarSubastaException>(() =>
                _consumer.Consume(contexto));
        }

    }
}
