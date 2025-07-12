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
    public class SubastaModificadaConsumerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly SubastaModificadaConsumer _consumer;

        public SubastaModificadaConsumerTest()
        {
            _consumer = new SubastaModificadaConsumer(_subastaServiceMock.Object);
        }

        [Fact]
        public async Task Consume_DeberiaModificarSubasta_CuandoOperacionEsExitosa()
        {
            var subastaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta Actualizada", "Descripción modificada",
                Guid.NewGuid(), DateTime.UtcNow.AddDays(-3), DateTime.UtcNow.AddDays(2), 100, 500, "Programada");

            var evento = new SubastaModificadaEvent
            (
                 subasta,
                usuarioId
            );

            var contextoMock = new Mock<ConsumeContext<SubastaModificadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x =>
                    x.ModificarSubastaMongoAsync(subasta, usuarioId))
                .ReturnsAsync(HttpStatusCode.OK);



            await _consumer.Consume(contextoMock.Object);

            _subastaServiceMock.Verify(x =>
                x.ModificarSubastaMongoAsync(subasta, usuarioId), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlModificarSubastaException_CuandoServicioFalla()
        {
            var subastaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta Fallida", "Descripción",
                Guid.NewGuid(), DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), 150, 1000, "Activa");

            var evento = new SubastaModificadaEvent
            (
                 subasta,
                usuarioId
            );

            var contextoMock = new Mock<ConsumeContext<SubastaModificadaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x =>
                x.ModificarSubastaMongoAsync(subasta, usuarioId))
                .ThrowsAsync(new Exception("Error al modificar"));

            await Assert.ThrowsAsync<FalloAlModificarSubastaException>(() =>
                _consumer.Consume(contextoMock.Object));
        }

    }
}
