using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.Consumers;
using MassTransit;
using Moq;

namespace TestMicroservicioSubasta.ConsumersTest
{
    public class SubastaRegistradaConsumerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly SubastaRegistradaConsumer _consumer;

        public SubastaRegistradaConsumerTest()
        {
            _consumer = new SubastaRegistradaConsumer(_subastaServiceMock.Object);
        }

        [Fact]
        public async Task Consume_DeberiaRegistrarSubasta_CuandoOperacionEsExitosa()
        {
            var subastaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta Test", "Descripción",
                Guid.NewGuid(), DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2), 100, 500, "Programada");

            var evento = new SubastaRegistradaEvent
            (
                subasta,
                usuarioId
            );

            var contextoMock = new Mock<ConsumeContext<SubastaRegistradaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x =>
                x.RegistrarSubastaMongoAsync(subasta, usuarioId))
                .ReturnsAsync(HttpStatusCode.OK);

            await _consumer.Consume(contextoMock.Object);

            _subastaServiceMock.Verify(x =>
                x.RegistrarSubastaMongoAsync(subasta, usuarioId), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlRegistrarSubastaException_CuandoServicioFalla()
        {
            var subastaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta Test", "Descripción",
                Guid.NewGuid(), DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(2), 150, 1000, "Programada");

            var evento = new SubastaRegistradaEvent
            (
                 subasta,
                 usuarioId
            );

            var contextoMock = new Mock<ConsumeContext<SubastaRegistradaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x =>
                x.RegistrarSubastaMongoAsync(subasta, usuarioId))
                .ThrowsAsync(new Exception("Error interno"));

            await Assert.ThrowsAsync<FalloAlRegistrarSubastaException>(() =>
                _consumer.Consume(contextoMock.Object));
        }

    }
}
