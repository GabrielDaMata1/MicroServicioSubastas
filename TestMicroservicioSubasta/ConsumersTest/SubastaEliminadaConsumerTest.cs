using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SubastaEliminadaConsumerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly SubastaEliminadaConsumer _consumer;

        public SubastaEliminadaConsumerTest()
        {
            _consumer = new SubastaEliminadaConsumer(_subastaServiceMock.Object);
        }

        [Fact]
        public async Task Consume_DeberiaEliminarSubasta_CuandoOperacionEsExitosa()
        {
            var subastaId = Guid.NewGuid();
            var evento = new SubastaEliminadaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<SubastaEliminadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x =>
                x.EliminarSubastaMongoAsync(subastaId)).ReturnsAsync(true);

            await _consumer.Consume(contextoMock.Object);

            _subastaServiceMock.Verify(x =>
                x.EliminarSubastaMongoAsync(subastaId), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlEliminarSubastaException_CuandoServicioFalla()
        {
            var subastaId = Guid.NewGuid();
            var evento = new SubastaEliminadaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<SubastaEliminadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x =>
                x.EliminarSubastaMongoAsync(subastaId)).ThrowsAsync(new Exception("Error interno"));

            await Assert.ThrowsAsync<FalloAlEliminarSubastaException>(() =>
                _consumer.Consume(contextoMock.Object));
        }

    }
}
