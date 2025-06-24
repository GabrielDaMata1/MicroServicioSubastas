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
using MassTransit;

namespace Infrastructure.Consumers
{
    public class SubastaAcabadaConsumer : IConsumer<SubastaAcabadaEvent>
    {
        private readonly ISubastaService _subastaService;
        private readonly IPujaService _pujaService;
        private readonly IPublishEndpoint _publish;
        private readonly IUsuarioService _usuarioService;
        private readonly IProductoService _productoService;



        public SubastaAcabadaConsumer(ISubastaService subastaService, IPublishEndpoint publish, IPujaService pujaService, IProductoService productoService, IUsuarioService usuarioService)
        {
            _subastaService = subastaService;
            _publish = publish;
            _pujaService = pujaService;
            _productoService = productoService;
            _usuarioService= usuarioService;
        }

        public async Task Consume(ConsumeContext<SubastaAcabadaEvent> context)
        {
            try
            {

                var resul = await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(context.Message.SubastaId, "Ended");

                if (resul == HttpStatusCode.OK)
                  await _subastaService.ActualizarEstadoSubastaMongoAsync(context.Message.SubastaId, "Ended");

                var subasta = await _subastaService.ObtenerSubastaPorIdMongoAsync(context.Message.SubastaId);

                var pujaGanadora = await _pujaService.ObtenerPujaGanadoraPorIdSubasta(context.Message.SubastaId);

                var historialSubasta = HistorialSubastaFactory.CrearHistorialSubasta(pujaGanadora.IdUsuario,
                    pujaGanadora.IdSubasta, pujaGanadora.MontoPuja.montoPuja);


                var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

                var idUsuario = await _productoService.ObtenerUsuarioIdPorIdProductoAsync(producto.Id);

                var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(idUsuario);

                if (pujaGanadora.MontoPuja.montoPuja < subasta.precioReservaSubasta.precioReserva)
                {
                    var idHistorialSubastaNoGanadora = await _subastaService.RegistrarHistorialSubastaPostgreSQLAsync(historialSubasta,"No Ganador");
                    if (idHistorialSubastaNoGanadora == Guid.Empty)
                        throw new FalloAlRegistrarHistorialSubastaException();
                    await _subastaService.RegistrarHistorialSubastaMongoAsync(historialSubasta,"No Ganador");

                    var modificarEstadoProducto = await _productoService.ModificarProductoAsync(correoUsuario, producto, "Disponible");

                    if (!modificarEstadoProducto)
                        throw new FalloAlModificarProductoException();
                }

                if (pujaGanadora.MontoPuja.montoPuja > subasta.precioReservaSubasta.precioReserva)
                {
                    var idHistorialSubastaGanadora = await _subastaService.RegistrarHistorialSubastaPostgreSQLAsync(historialSubasta, "Ganador");
                    if (idHistorialSubastaGanadora == Guid.Empty)
                        throw new FalloAlRegistrarHistorialSubastaException();
                    await _subastaService.RegistrarHistorialSubastaMongoAsync(historialSubasta, "Ganador");
                    var modificarEstadoProducto = await _productoService.ModificarProductoAsync(correoUsuario, producto, "Subastado");

                    if (!modificarEstadoProducto)
                        throw new FalloAlModificarProductoException();
                }

            }
            catch (FalloAlRegistrarHistorialSubastaException)
            {
                throw;
            }
            catch (FalloAlModificarProductoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FalloAlModificarSubastaException("Ocurrió un error al modificar la subasta. en la base de datos", ex);
            }
        }
    }
}
