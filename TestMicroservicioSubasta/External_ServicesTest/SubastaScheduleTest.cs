using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.External_Services.Hangfire;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;
using Moq;

namespace TestMicroservicioSubasta.External_ServicesTest
{
    public class SubastaScheduleTest
    {
        private readonly Mock<IBus> _busMock = new();
        private readonly Mock<ISubastaJobRepository> _jobRepoMock = new();
        private readonly SubastaSchedule _schedule;

        public SubastaScheduleTest()
        {
            _schedule = new SubastaSchedule(_busMock.Object, _jobRepoMock.Object);
        }

        [Fact]
        public async Task PublicarInicio_DeberiaPublicarEventoSubastaIniciada()
        {
            var id = Guid.NewGuid();
            var inicio = DateTime.UtcNow;
            var fin = DateTime.UtcNow.AddDays(3);

            await _schedule.PublicarInicio(id, inicio, fin);

            _busMock.Verify(b => b.Publish(It.Is<SubastaIniciadaEvent>(e =>
                e.SubastaId == id && e.fechaInicio == inicio && e.fechaFin == fin), default), Times.Once);
        }

        [Fact]
        public async Task PublicarFin_DeberiaPublicarEventoSubastaFinalizada()
        {
            var id = Guid.NewGuid();
            var inicio = DateTime.UtcNow;
            var fin = DateTime.UtcNow.AddDays(2);

            await _schedule.PublicarFin(id, inicio, fin);

            _busMock.Verify(b => b.Publish(It.Is<SubastaFinalizadaEvent>(e =>
                e.SubastaId == id && e.fechaInicio == inicio && e.fechaFin == fin), default), Times.Once);
        }

        [Fact]
        public async Task PublicarTemporizadorPago_DeberiaPublicarEventoTemporizadorCancelarPago()
        {
            var id = Guid.NewGuid();

            await _schedule.PublicarTemporizadorPago(id);

            _busMock.Verify(b => b.Publish(It.Is<TemporizadorCancelarPagoEvent>(e =>
                e.SubastaId == id), default), Times.Once);
        }


        [Fact]
        public async Task EliminarTemporizadorPagoSubasta_NoDeberiaEliminarJobCuandoJobIdEsVacio()
        {
            var id = Guid.NewGuid();
            _jobRepoMock.Setup(r => r.ObtenerJobIdAsync(id, "Pago")).ReturnsAsync(string.Empty);

            await _schedule.EliminarTemporizadorPagoSubasta(id);

            _jobRepoMock.Verify(r => r.EliminarJobIdAsync(It.IsAny<Guid>(), "Pago"), Times.Never);
        }

    }
}
