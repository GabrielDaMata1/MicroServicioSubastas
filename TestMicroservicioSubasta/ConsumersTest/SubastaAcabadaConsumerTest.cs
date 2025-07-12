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
using Domain.Value_Objects;
using Infrastructure.Consumers;
using MassTransit;
using Moq;

namespace TestMicroservicioSubasta.ConsumersTest
{
    public class SubastaAcabadaConsumerTest
    {
        private readonly Mock<ISubastaService> _subastaServiceMock = new();
        private readonly Mock<IPujaService> _pujaServiceMock = new();
        private readonly Mock<IPublishEndpoint> _publishMock = new();
        private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
        private readonly Mock<IProductoService> _productoServiceMock = new();
        private readonly Mock<INotificacionService> _notificacionServiceMock = new();

        private readonly SubastaAcabadaConsumer _consumer;

        public SubastaAcabadaConsumerTest()
        {
            _consumer = new SubastaAcabadaConsumer(
                _subastaServiceMock.Object,
                _publishMock.Object,
                _pujaServiceMock.Object,
                _productoServiceMock.Object,
                _usuarioServiceMock.Object,
                _notificacionServiceMock.Object
            );
        }

        [Fact]
        public async Task Consume_DeberiaActualizarSubastaYProducto_CuandoNoHayPujaGanadora()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var correoUsuario = "subastador@correo.com";

            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta sin pujas", "Desc", productoId,
                DateTime.UtcNow.AddDays(-3), DateTime.UtcNow.AddDays(-1), 100, 1000, "Activa");

            var producto = new Producto(productoId, new NombreProductoVO("Libro"), new DescripcionProductoVO("Antiguo"),
                new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Libros"),
                new EstadoProductoVO("Disponible"));

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subasta);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);
            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId)).ReturnsAsync(Guid.NewGuid());
            _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(correoUsuario);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _pujaServiceMock.Setup(x => x.ObtenerPujaGanadoraPorIdSubasta(subastaId)).ReturnsAsync((Puja)null);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Deserted")).ReturnsAsync(HttpStatusCode.OK);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Deserted")).ReturnsAsync(HttpStatusCode.OK);
            _productoServiceMock.Setup(x => x.ModificarProductoAsync(correoUsuario, producto, "Disponible")).ReturnsAsync(true);

            var contexto = Mock.Of<ConsumeContext<SubastaAcabadaEvent>>(c => c.Message == new SubastaAcabadaEvent (subastaId));

            await _consumer.Consume(contexto);

            _subastaServiceMock.Verify(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Deserted"), Times.Once);
            _productoServiceMock.Verify(x => x.ModificarProductoAsync(correoUsuario, producto, "Disponible"), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlRegistrarHistorialSubastaException_CuandoRegistroFalla()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta", "Desc", productoId,
                DateTime.UtcNow.AddDays(-3), DateTime.UtcNow.AddDays(-1), 50, 3000, "Activa");

            var puja = new Puja(Guid.NewGuid(), usuarioId, subastaId, new MontoPujaVO(2800),
                new MontoMaximoPujaVO(3000), new TipoPujaVO("Manual"), new MontoPredeterminadoPujaVO(200),
                new FechaPujaVO(DateTime.UtcNow));

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subasta);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(
                new Producto(productoId, new NombreProductoVO("Objeto"), new DescripcionProductoVO("Desc"),
                new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(3000), new CategoriaProductoVO("Otros"),
                new EstadoProductoVO("Disponible")));

            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId)).ReturnsAsync(usuarioId);
            _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync("subastador@correo.com");

            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);

            _pujaServiceMock.Setup(x => x.ObtenerPujaGanadoraPorIdSubasta(subastaId)).ReturnsAsync(puja);
            _subastaServiceMock.Setup(x => x.RegistrarHistorialSubastaPostgreSQLAsync(It.IsAny<HistorialSubasta>(), "No Ganador")).ReturnsAsync(Guid.Empty);

            var contexto = Mock.Of<ConsumeContext<SubastaAcabadaEvent>>(c => c.Message == new SubastaAcabadaEvent (subastaId));

            await Assert.ThrowsAsync<FalloAlRegistrarHistorialSubastaException>(() => _consumer.Consume(contexto));
        }

        [Fact]
        public async Task Consume_DeberiaRegistrarHistorialComoNoGanador_CuandoPujaNoAlcanzaReserva()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta", "Desc", productoId,
                DateTime.UtcNow.AddDays(-4), DateTime.UtcNow.AddDays(-1), 50, 500, "Finalizada");

            var producto = new Producto(productoId, new NombreProductoVO("Bicicleta"), new DescripcionProductoVO("Ruta"),
                new ImagenURLProductoVO("img"), new PrecioBaseProductoVO(400), new CategoriaProductoVO("Deportes"),
                new EstadoProductoVO("Disponible"));

            var puja = new Puja(Guid.NewGuid(), usuarioId, subastaId, new MontoPujaVO(450),
                new MontoMaximoPujaVO(470), new TipoPujaVO("Manual"), new MontoPredeterminadoPujaVO(10),
                new FechaPujaVO(DateTime.UtcNow));

            var correo = "vendedor@correo.com";

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subasta);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);
            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId)).ReturnsAsync(usuarioId);
            _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(usuarioId)).ReturnsAsync(correo);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _pujaServiceMock.Setup(x => x.ObtenerPujaGanadoraPorIdSubasta(subastaId)).ReturnsAsync(puja);
            _subastaServiceMock.Setup(x => x.RegistrarHistorialSubastaPostgreSQLAsync(It.IsAny<HistorialSubasta>(), "No Ganador")).ReturnsAsync(Guid.NewGuid());
            _subastaServiceMock.Setup(x => x.RegistrarHistorialSubastaMongoAsync(It.IsAny<HistorialSubasta>(), "No Ganador")).ReturnsAsync(HttpStatusCode.OK);
            _productoServiceMock.Setup(x => x.ModificarProductoAsync(correo, producto, "Disponible")).ReturnsAsync(true);

            var contexto = Mock.Of<ConsumeContext<SubastaAcabadaEvent>>(x => x.Message == new SubastaAcabadaEvent (subastaId));

            await _consumer.Consume(contexto);

            _subastaServiceMock.Verify(x =>
                x.RegistrarHistorialSubastaPostgreSQLAsync(It.IsAny<HistorialSubasta>(), "No Ganador"), Times.Once);

            _productoServiceMock.Verify(x =>
                x.ModificarProductoAsync(correo, producto, "Disponible"), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlModificarProductoException_CuandoModificacionProductoFalla()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta", "Desc", productoId,
                DateTime.UtcNow.AddDays(-4), DateTime.UtcNow.AddDays(-1), 50, 100, "Finalizada");

            var producto = new Producto(productoId, new NombreProductoVO("Monitor"), new DescripcionProductoVO("LED"),
                new ImagenURLProductoVO("img"), new PrecioBaseProductoVO(95), new CategoriaProductoVO("Tecnología"),
                new EstadoProductoVO("Disponible"));

            var puja = new Puja(Guid.NewGuid(), usuarioId, subastaId, new MontoPujaVO(120),
                new MontoMaximoPujaVO(150), new TipoPujaVO("Automática"), new MontoPredeterminadoPujaVO(5),
                new FechaPujaVO(DateTime.UtcNow));

            var correoGanador = "usuario@correo.com";
            var correoSubastador = "vendedor@correo.com";

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subasta);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(producto);
            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId)).ReturnsAsync(usuarioId);
            _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(usuarioId)).ReturnsAsync(correoSubastador);
            _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(It.Is<Guid>(id => id != usuarioId))).ReturnsAsync(correoGanador);

            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _pujaServiceMock.Setup(x => x.ObtenerPujaGanadoraPorIdSubasta(subastaId)).ReturnsAsync(puja);
            _subastaServiceMock.Setup(x => x.RegistrarHistorialSubastaPostgreSQLAsync(It.IsAny<HistorialSubasta>(), "Ganador")).ReturnsAsync(Guid.NewGuid());
            _subastaServiceMock.Setup(x => x.RegistrarHistorialSubastaMongoAsync(It.IsAny<HistorialSubasta>(), "Ganador")).ReturnsAsync(HttpStatusCode.OK);

            // 🚫 Simula que falla la modificación del producto
            _productoServiceMock.Setup(x => x.ModificarProductoAsync(correoSubastador, producto, "Subastado")).ReturnsAsync(false);

            var contexto = Mock.Of<ConsumeContext<SubastaAcabadaEvent>>(x => x.Message == new SubastaAcabadaEvent (subastaId));

            await Assert.ThrowsAsync<FalloAlModificarProductoException>(() => _consumer.Consume(contexto));
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlModificarSubastaException_CuandoOcurreErrorNoControlado()
        {
            var subastaId = Guid.NewGuid();

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ThrowsAsync(new Exception("Fallo de infraestructura"));

            var contexto = Mock.Of<ConsumeContext<SubastaAcabadaEvent>>(x => x.Message == new SubastaAcabadaEvent (subastaId));

            await Assert.ThrowsAsync<FalloAlModificarSubastaException>(() => _consumer.Consume(contexto));
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlModificarSubastaException_CuandoFallaMongoAlFinal()
        {
            var subastaId = Guid.NewGuid();
            var productoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var subasta = SubastaFactory.CrearSubastaConId(subastaId, "Subasta Fallida", "Desc", productoId,
                DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(-1), 100, 500, "Activa");

            var pujaGanadora = new Puja(Guid.NewGuid(), usuarioId, subastaId,
                new MontoPujaVO(600), new MontoMaximoPujaVO(650), new TipoPujaVO("Manual"),
                new MontoPredeterminadoPujaVO(30), new FechaPujaVO(DateTime.UtcNow));

            _subastaServiceMock.Setup(x => x.ObtenerSubastaPorIdMongoAsync(subastaId)).ReturnsAsync(subasta);
            _productoServiceMock.Setup(x => x.ObtenerProductoPorGuid(productoId)).ReturnsAsync(
                new Producto(productoId, new NombreProductoVO("Consola"), new DescripcionProductoVO("Gaming"),
                new ImagenURLProductoVO("img"), new PrecioBaseProductoVO(300), new CategoriaProductoVO("Videojuegos"),
                new EstadoProductoVO("Disponible")));
            _productoServiceMock.Setup(x => x.ObtenerUsuarioIdPorIdProductoAsync(productoId)).ReturnsAsync(usuarioId);
            _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync("correo@ejemplo.com");

            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaPostgreSQLAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _subastaServiceMock.Setup(x => x.ActualizarEstadoSubastaMongoAsync(subastaId, "Ended")).ReturnsAsync(HttpStatusCode.OK);
            _pujaServiceMock.Setup(x => x.ObtenerPujaGanadoraPorIdSubasta(subastaId)).ReturnsAsync(pujaGanadora);
            _subastaServiceMock.Setup(x => x.RegistrarHistorialSubastaPostgreSQLAsync(It.IsAny<HistorialSubasta>(), "Ganador")).ReturnsAsync(Guid.NewGuid());

            _subastaServiceMock.Setup(x => x.RegistrarHistorialSubastaMongoAsync(It.IsAny<HistorialSubasta>(), "Ganador")).ThrowsAsync(new Exception("Mongo out"));

            var contexto = Mock.Of<ConsumeContext<SubastaAcabadaEvent>>(c => c.Message == new SubastaAcabadaEvent ( subastaId ));

            await Assert.ThrowsAsync<FalloAlModificarSubastaException>(() =>
                _consumer.Consume(contexto));

        }
    }
}