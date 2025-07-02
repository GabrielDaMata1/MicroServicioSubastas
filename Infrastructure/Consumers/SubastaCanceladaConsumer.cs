using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Service;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    public class SubastaCanceladaConsumer : IConsumer<SubastaCanceladaEvent>
    {
        private readonly ISubastaService _subastaService;
        private readonly ISubastaSchedule _subastaSchedule;
        private readonly IProductoService _productoService;
        private readonly IUsuarioService _usuarioService;

        public SubastaCanceladaConsumer(ISubastaService subastaService, ISubastaSchedule subastaSchedule, IProductoService productoService, IUsuarioService usuarioService)
        {
            _subastaService = subastaService;
            _subastaSchedule = subastaSchedule;
            _productoService = productoService;
            _usuarioService = usuarioService;
        }

        public async Task Consume(ConsumeContext<SubastaCanceladaEvent> context)
        {
          try
          {
            var id = context.Message.idSubasta;

            var resul = await _subastaService.ActualizarEstadoSubastaPostgreSQLAsync(id, "Canceled");
                if (resul != HttpStatusCode.OK)
                    throw new FalloAlModificarSubastaException();  

            await _subastaService.ActualizarEstadoSubastaMongoAsync(id, "Canceled");

            var subasta = await _subastaService.ObtenerSubastaPorIdMongoAsync(id);
            
              if (subasta==null)
                 throw new FalloAlObtenerSubastasException();

            var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

              if (producto == null)
                  throw new FalloAlObtenerProductoException();

            var idUsuario = await _productoService.ObtenerUsuarioIdPorIdProductoAsync(producto.Id);

              if (idUsuario==Guid.Empty)
                  throw new FalloAlObtenerUsuarioException();

            var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(idUsuario);

              if (correoUsuario == null)
                  throw new FalloAlObtenerUsuarioException();

            var modificarEstadoProducto = await _productoService.ModificarProductoAsync(correoUsuario, producto, "Disponible");

              if (!modificarEstadoProducto)
                  throw new FalloAlModificarProductoException();
          }
          catch (FalloAlModificarSubastaException)
          {
             throw;
          }
          catch (FalloAlObtenerSubastasException)
          {
             throw;
          }
          catch (FalloAlObtenerProductoException)
          {
             throw;
          }
          catch (FalloAlObtenerUsuarioException)
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
