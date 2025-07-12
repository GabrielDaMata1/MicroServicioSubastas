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
    public class SubastaCompletadaConsumerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<ISubastaSchedule> _subastaScheduleMock = new();
        private readonly SubastaCompletadaConsumer _consumer;

        public SubastaCompletadaConsumerTest()
        {
            _consumer = new SubastaCompletadaConsumer(_subastaServiceMock.Object, _subastaScheduleMock.Object);
        }

        [Fact]
        public async Task Consume_DeberiaActualizarPostgreSQLYMongoYEliminarTemporizador_CuandoTodoEsExitoso()
        {
            var subastaId = Guid.NewGuid();
            var evento = new SubastaCompletadaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<SubastaCompletadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Completed"))
                .ReturnsAsync(HttpStatusCode.OK);

            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Completed"))
                .ReturnsAsync(HttpStatusCode.OK);

            _subastaScheduleMock.Setup(x => x.EliminarTemporizadorPagoSubasta(subastaId))
                .Returns(Task.CompletedTask);

            await _consumer.Consume(contextoMock.Object);

            _subastaServiceMock.Verify(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Completed"), Times.Once);
            _subastaServiceMock.Verify(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Completed"), Times.Once);
            _subastaScheduleMock.Verify(x => x.EliminarTemporizadorPagoSubasta(subastaId), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaActualizarSoloPostgreSQLYEliminarTemporizador_CuandoMongoSeOmite()
        {
            var subastaId = Guid.NewGuid();
            var evento = new SubastaCompletadaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<SubastaCompletadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Completed"))
                .ReturnsAsync(HttpStatusCode.BadRequest);

            _subastaScheduleMock.Setup(x => x.EliminarTemporizadorPagoSubasta(subastaId))
                .Returns(Task.CompletedTask);

            await _consumer.Consume(contextoMock.Object);

            _subastaServiceMock.Verify(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Completed"), Times.Once);
            _subastaServiceMock.Verify(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Completed"), Times.Never);
            _subastaScheduleMock.Verify(x => x.EliminarTemporizadorPagoSubasta(subastaId), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlModificarSubastaException_CuandoServicioFalla()
        {
            var subastaId = Guid.NewGuid();
            var evento = new SubastaCompletadaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<SubastaCompletadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Completed"))
                .ThrowsAsync(new Exception("Error interno"));

            await Assert.ThrowsAsync<FalloAlModificarSubastaException>(() =>
                _consumer.Consume(contextoMock.Object));
        }

    }
}
