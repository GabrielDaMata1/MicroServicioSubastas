using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Service;
using Domain.Entities;
using Domain.Factory;
using Domain.Interfaces;
using Domain.Value_Object;
using Moq;

namespace TestMicroservicioSubasta.Services
{
    public class SubastaServiceTest
    {
        private readonly Mock<ISubastaRepositoryMongo> _mongoRepoMock = new();
        private readonly Mock<ISubastaRepositoryPostgreSQL> _pgRepoMock = new();
        private readonly Mock<IHistorialSubastaMongoRepository> _histMongoRepoMock = new();
        private readonly Mock<IHistorialSubastaPostgreSQLRepository> _histPgRepoMock = new();

        private readonly SubastaService _service;

        public SubastaServiceTest()
        {
            _service = new SubastaService(
                _mongoRepoMock.Object,
                _pgRepoMock.Object,
                _histMongoRepoMock.Object,
                _histPgRepoMock.Object
            );
        }

        [Fact]
        public async Task RegistrarSubastaPostgreSQLAsync_DeberiaRetornarGuid_CuandoRegistroExitoso()
        {
            var subasta = SubastaFactory.CrearSubasta("Subasta Test", "Desc", Guid.NewGuid(), DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(3), 100, 2000, "Programada");
            var idUsuario = Guid.NewGuid();
            var subastaId = Guid.NewGuid();

            _pgRepoMock.Setup(x => x.RegistrarSubastaAsync(subasta, idUsuario)).ReturnsAsync(subastaId);

            var result = await _service.RegistrarSubastaPostgreSQLAsync(subasta, idUsuario);

            Assert.Equal(subastaId, result);
        }

        [Fact]
        public async Task RegistrarSubastaMongoAsync_DeberiaRetornarStatus_CuandoRegistroExitoso()
        {
            var subasta = SubastaFactory.CrearSubasta("Subasta Mongo", "Info", Guid.NewGuid(), DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(2), 50, 1500, "Programada");
            var idUsuario = Guid.NewGuid();

            _mongoRepoMock.Setup(x => x.RegistrarSubastaAsync(subasta, idUsuario)).ReturnsAsync(HttpStatusCode.Created);

            var result = await _service.RegistrarSubastaMongoAsync(subasta, idUsuario);

            Assert.Equal(HttpStatusCode.Created, result);
        }

        [Fact]
        public async Task ModificarSubastaPostgreSQLAsync_DeberiaRetornarOK_CuandoModificacionExitosa()
        {
            var subasta = SubastaFactory.CrearSubasta("Update Test", "Info", Guid.NewGuid(), DateTime.UtcNow.AddDays(2),
                DateTime.UtcNow.AddDays(4), 80, 1800, "Programada");
            var usuarioId = Guid.NewGuid();

            _pgRepoMock.Setup(x => x.ModificarSubastaAsync(subasta, usuarioId)).ReturnsAsync(HttpStatusCode.OK);

            var result = await _service.ModificarSubastaPostgreSQLAsync(subasta, usuarioId);

            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task ModificarSubastaMongoAsync_DeberiaRetornarOK_CuandoModificacionExitosa()
        {
            var subasta = SubastaFactory.CrearSubasta("Update Mongo", "Desc", Guid.NewGuid(), DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(2), 60, 1600, "Programada");
            var usuarioId = Guid.NewGuid();

            _mongoRepoMock.Setup(x => x.ModificarSubastaAsync(subasta, usuarioId)).ReturnsAsync(HttpStatusCode.OK);

            var result = await _service.ModificarSubastaMongoAsync(subasta, usuarioId);

            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task ObtenerSubastaPorIdMongoAsync_DeberiaRetornarSubasta_CuandoExiste()
        {
            var subastaId = Guid.NewGuid();
            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Consulta", "Det", Guid.NewGuid(),
                DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), 40, 1300, "Programada");

            _mongoRepoMock.Setup(x => x.ObtenerSubastaPorId(subastaId)).ReturnsAsync(subasta);

            var result = await _service.ObtenerSubastaPorIdMongoAsync(subastaId);

            Assert.NotNull(result);
            Assert.Equal("Consulta", result.nombreSubasta.Nombre);
        }

        [Fact]
        public async Task EliminarSubastaMongoAsync_DeberiaRetornarTrue_CuandoEliminacionExitosa()
        {
            var subastaId = Guid.NewGuid();
            _mongoRepoMock.Setup(x => x.EliminarSubastaAsync(subastaId)).ReturnsAsync(true);

            var result = await _service.EliminarSubastaMongoAsync(subastaId);

            Assert.True(result);
        }

        [Fact]
        public async Task RegistrarSubastaPostgreSQLAsync_DeberiaLanzarExcepcion_CuandoFalla()
        {
            var subasta = SubastaFactory.CrearSubasta("Error", "Postgres", Guid.NewGuid(), DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(2), 100, 2000, "Programada");
            var usuarioId = Guid.NewGuid();

            _pgRepoMock.Setup(x => x.RegistrarSubastaAsync(subasta, usuarioId)).ThrowsAsync(new Exception("Fallo"));

            await Assert.ThrowsAsync<PostgresRepositoryException>(() =>
                _service.RegistrarSubastaPostgreSQLAsync(subasta, usuarioId));
        }
        [Fact]
        public async Task ModificarSubastaPostgreSQLAsync_DeberiaLanzarPostgresRepositoryException_CuandoFalla()
        {
            var subasta = SubastaFactory.CrearSubasta("Mod", "DB", Guid.NewGuid(), DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(2), 100, 2000, "Activa");
            var usuarioId = Guid.NewGuid();

            _pgRepoMock.Setup(x => x.ModificarSubastaAsync(subasta, usuarioId)).ThrowsAsync(new Exception("Error PG"));

            await Assert.ThrowsAsync<PostgresRepositoryException>(() =>
                _service.ModificarSubastaPostgreSQLAsync(subasta, usuarioId));
        }

        [Fact]
        public async Task RegistrarSubastaMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var subasta = SubastaFactory.CrearSubasta("Mongo", "Crear", Guid.NewGuid(), DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(2), 100, 2000, "Programada");
            var usuarioId = Guid.NewGuid();

            _mongoRepoMock.Setup(x => x.RegistrarSubastaAsync(subasta, usuarioId)).ThrowsAsync(new Exception("Error Mongo"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.RegistrarSubastaMongoAsync(subasta, usuarioId));
        }

        [Fact]
        public async Task ModificarSubastaMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var subasta = SubastaFactory.CrearSubasta("Mongo", "Mod", Guid.NewGuid(), DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(2), 100, 2000, "Activa");
            var usuarioId = Guid.NewGuid();

            _mongoRepoMock.Setup(x => x.ModificarSubastaAsync(subasta, usuarioId)).ThrowsAsync(new Exception("ModError"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ModificarSubastaMongoAsync(subasta, usuarioId));
        }

        [Fact]
        public async Task ObtenerSubastaPorIdMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var subastaId = Guid.NewGuid();
            _mongoRepoMock.Setup(x => x.ObtenerSubastaPorId(subastaId)).ThrowsAsync(new Exception("QueryFail"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ObtenerSubastaPorIdMongoAsync(subastaId));
        }

        [Fact]
        public async Task EliminarSubastaMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var subastaId = Guid.NewGuid();
            _mongoRepoMock.Setup(x => x.EliminarSubastaAsync(subastaId)).ThrowsAsync(new Exception("DeleteFail"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.EliminarSubastaMongoAsync(subastaId));
        }

        [Fact]
        public async Task EliminarSubastaPostgreSQLAsync_DeberiaRetornarTrue_CuandoEliminacionExitosa()
        {
            var id = Guid.NewGuid();
            _pgRepoMock.Setup(x => x.EliminarSubastaAsync(id)).ReturnsAsync(true);

            var result = await _service.EliminarSubastaPostgreSQLAsync(id);
            Assert.True(result);
        }

        [Fact]
        public async Task EliminarSubastaPostgreSQLAsync_DeberiaLanzarPostgresRepositoryException_CuandoFalla()
        {
            var id = Guid.NewGuid();
            _pgRepoMock.Setup(x => x.EliminarSubastaAsync(id)).ThrowsAsync(new Exception("Falla"));

            await Assert.ThrowsAsync<PostgresRepositoryException>(() => _service.EliminarSubastaPostgreSQLAsync(id));
        }

        [Fact]
        public async Task ObtenerUsuarioIdPorSubastaIdMongoAsync_DeberiaRetornarGuid_CuandoExitoso()
        {
            var subastaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            _mongoRepoMock.Setup(x => x.ObtenerUsuarioIdPorSubastaId(subastaId)).ReturnsAsync(usuarioId);

            var result = await _service.ObtenerUsuarioIdPorSubastaIdMongoAsync(subastaId);
            Assert.Equal(usuarioId, result);
        }

        [Fact]
        public async Task ObtenerUsuarioIdPorSubastaIdMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var subastaId = Guid.NewGuid();
            _mongoRepoMock.Setup(x => x.ObtenerUsuarioIdPorSubastaId(subastaId)).ThrowsAsync(new Exception("Error"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ObtenerUsuarioIdPorSubastaIdMongoAsync(subastaId));
        }

        [Fact]
        public async Task ObtenerSubastasMongo_DeberiaRetornarLista_CuandoExitoso()
        {
            var lista = new List<Subasta> { SubastaFactory.CrearSubasta("Titulo", "Desc", Guid.NewGuid(), DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), 50, 1500, "Activa") };
            _mongoRepoMock.Setup(x => x.ObtenerSubastas()).ReturnsAsync(lista);

            var result = await _service.ObtenerSubastasMongo();
            Assert.Single(result);
        }

        [Fact]
        public async Task ObtenerSubastasMongo_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            _mongoRepoMock.Setup(x => x.ObtenerSubastas()).ThrowsAsync(new Exception("error"));
            await Assert.ThrowsAsync<MongoRepositoryException>(() => _service.ObtenerSubastasMongo());
        }

        [Fact]
        public async Task ObtenerSubastaMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var id = Guid.NewGuid();
            _mongoRepoMock.Setup(x => x.ObtenerSubasta(id)).ThrowsAsync(new Exception("Error"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() => _service.ObtenerSubastaMongoAsync(id));
        }

        [Fact]
        public async Task ObtenerSubastasPorUsuarioMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var id = Guid.NewGuid();
            _mongoRepoMock.Setup(x => x.ObtenerSubastasPorUsuario(id)).ThrowsAsync(new Exception("Error"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ObtenerSubastasPorUsuarioMongoAsync(id));
        }

        [Fact]
        public async Task ActualizarEstadoSubastaMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            _mongoRepoMock.Setup(x => x.ActualizarEstadoSubasta(It.IsAny<Guid>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Fail"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ActualizarEstadoSubastaMongoAsync(Guid.NewGuid(), "Cancelada"));
        }

        [Fact]
        public async Task ActualizarEstadoSubastaPostgreSQLAsync_DeberiaLanzarPostgresRepositoryException_CuandoFalla()
        {
            _pgRepoMock.Setup(x => x.ActualizarEstadoSubasta(It.IsAny<Guid>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Fail"));

            await Assert.ThrowsAsync<PostgresRepositoryException>(() =>
                _service.ActualizarEstadoSubastaPostgreSQLAsync(Guid.NewGuid(), "Cancelada"));
        }

        [Fact]
        public async Task RegistrarHistorialSubastaMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var historial = new HistorialSubasta(Guid.NewGuid(), Guid.NewGuid(), new MontoFinalSubastaVO(2500));

            _histMongoRepoMock.Setup(x => x.registrarHistorialSubastaAsync(historial, "Ganada"))
                .ThrowsAsync(new Exception("MongoFail"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.RegistrarHistorialSubastaMongoAsync(historial, "Ganada"));
        }

        [Fact]
        public async Task RegistrarHistorialSubastaPostgreSQLAsync_DeberiaRetornarGuid_CuandoExitoso()
        {
            var historial = new HistorialSubasta(Guid.NewGuid(), Guid.NewGuid(), new MontoFinalSubastaVO(2200));
            var id = Guid.NewGuid();

            _histPgRepoMock.Setup(x => x.registrarHistorialSubastaAsync(historial, "Ganada")).ReturnsAsync(id);

            var result = await _service.RegistrarHistorialSubastaPostgreSQLAsync(historial, "Ganada");

            Assert.Equal(id, result);
        }

        [Fact]
        public async Task RegistrarHistorialSubastaPostgreSQLAsync_DeberiaLanzarPostgresRepositoryException_CuandoFalla()
        {
            var historial = new HistorialSubasta(Guid.NewGuid(), Guid.NewGuid(), new MontoFinalSubastaVO(2200));

            _histPgRepoMock.Setup(x => x.registrarHistorialSubastaAsync(historial, "Ganada"))
                .ThrowsAsync(new Exception("PG error"));

            await Assert.ThrowsAsync<PostgresRepositoryException>(() =>
                _service.RegistrarHistorialSubastaPostgreSQLAsync(historial, "Ganada"));
        }

        [Fact]
        public async Task ObtenerSubastasGanadasPorUsuarioMongoAsync_DeberiaRetornarLista_CuandoExitoso()
        {
            var id = Guid.NewGuid();
            var lista = new List<HistorialSubasta> { new HistorialSubasta(id, Guid.NewGuid(), new MontoFinalSubastaVO(3000)) };

            _histMongoRepoMock.Setup(x => x.ObtenerSubastasGanadasPorUsuario(id)).ReturnsAsync(lista);

            var result = await _service.ObtenerSubastasGanadasPorUsuarioMongoAsync(id);

            Assert.Single(result);
        }

        [Fact]
        public async Task ObtenerSubastasGanadasPorUsuarioMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var id = Guid.NewGuid();
            _histMongoRepoMock.Setup(x => x.ObtenerSubastasGanadasPorUsuario(id)).ThrowsAsync(new Exception("MongoFail"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ObtenerSubastasGanadasPorUsuarioMongoAsync(id));
        }

        [Fact]
        public async Task ObtenerSubastasGanadasDetalleMongoAsync_DeberiaRetornarLista_CuandoExitoso()
        {
            var id = Guid.NewGuid();
            var lista = new List<Subasta> { SubastaFactory.CrearSubasta("Detalle", "Desc", Guid.NewGuid(), DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), 100, 3000, "Ganada") };

            _mongoRepoMock.Setup(x => x.ObtenerSubastasGanadasDetalle(id)).ReturnsAsync(lista);

            var result = await _service.ObtenerSubastasGanadasDetalleMongoAsync(id);

            Assert.Single(result);
        }

        [Fact]
        public async Task ObtenerSubastasGanadasDetalleMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var id = Guid.NewGuid();
            _mongoRepoMock.Setup(x => x.ObtenerSubastasGanadasDetalle(id)).ThrowsAsync(new Exception("Error"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ObtenerSubastasGanadasDetalleMongoAsync(id));
        }

        [Fact]
        public async Task ObtenerSubastasGanadasMongoAsync_DeberiaRetornarLista_CuandoExitoso()
        {
            var lista = new List<Subasta> { SubastaFactory.CrearSubasta("Ganada", "Desc", Guid.NewGuid(), DateTime.UtcNow.AddDays(-3), DateTime.UtcNow, 100, 3500, "Finalizada") };

            _mongoRepoMock.Setup(x => x.ObtenerSubastasGanadas()).ReturnsAsync(lista);

            var result = await _service.ObtenerSubastasGanadasMongoAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task ObtenerSubastasGanadasMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            _mongoRepoMock.Setup(x => x.ObtenerSubastasGanadas()).ThrowsAsync(new Exception("Error"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ObtenerSubastasGanadasMongoAsync());
        }

        [Fact]
        public async Task ObtenerHistorialSubastaMongoAsync_DeberiaRetornarHistorial_CuandoExitoso()
        {
            var id = Guid.NewGuid();
            var historial = new HistorialSubasta(id, Guid.NewGuid(), new MontoFinalSubastaVO(2400));

            _histMongoRepoMock.Setup(x => x.ObtenerHistorialSubasta(id)).ReturnsAsync(historial);

            var result = await _service.ObtenerHistorialSubastaMongoAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.IdSubasta);
        }

        [Fact]
        public async Task ObtenerHistorialSubastaMongoAsync_DeberiaLanzarMongoRepositoryException_CuandoFalla()
        {
            var id = Guid.NewGuid();
            _histMongoRepoMock.Setup(x => x.ObtenerHistorialSubasta(id)).ThrowsAsync(new Exception("Error"));

            await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ObtenerHistorialSubastaMongoAsync(id));
        }

    }
}
