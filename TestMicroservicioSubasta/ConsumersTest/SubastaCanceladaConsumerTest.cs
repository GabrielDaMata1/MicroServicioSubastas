using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using Domain.Value_Object;
using Infrastructure.Consumers;
using MassTransit;
using Moq;

namespace TestMicroservicioSubasta.ConsumersTest
{
    public class SubastaCanceladaConsumerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<ISubastaSchedule> _subastaScheduleMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<INotificacionService> _notificacionServiceMock = new();

        private readonly SubastaCanceladaConsumer _consumer;

        public SubastaCanceladaConsumerTest()
        {
            _consumer = new SubastaCanceladaConsumer(
                _subastaServiceMock.Object,
                _subastaScheduleMock.Object,
                _productoServiceMock.Object,
                _usuarioServiceMock.Object,
                _notificacionServiceMock.Object
            );
        }


        [Fact]
        public async Task Consume_DeberiaLanzarExcepcion_CuandoPostgreSQLFalla()
        {
            var subastaId = Guid.NewGuid();
            var evento = new SubastaCanceladaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<SubastaCanceladaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x =>
                x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Canceled")).ReturnsAsync(HttpStatusCode.BadRequest);

            await Assert.ThrowsAsync<FalloAlModificarSubastaException>(() =>
                _consumer.Consume(contextoMock.Object));
        }

        [Fact]
        public async Task Consume_DeberiaLanzarExcepcion_CuandoSubastaNoExiste()
        {
            var subastaId = Guid.NewGuid();
            var evento = new SubastaCanceladaEvent (subastaId);
            var contextoMock = new Mock<ConsumeContext<SubastaCanceladaEvent>>();
            contextoMock.Setup(c => c.Message).Returns(evento);

            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Canceled")).ReturnsAsync(HttpStatusCode.OK);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Canceled")).ReturnsAsync(HttpStatusCode.OK);
            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync((Subasta)null);

            await Assert.ThrowsAsync<FalloAlObtenerSubastasException>(() =>
                _consumer.Consume(contextoMock.Object));
        }

       
    }
}
