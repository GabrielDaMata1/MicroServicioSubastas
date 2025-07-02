using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exceptions;
using Application.Querys;
using Domain.Interfaces;
using MediatR;

namespace Application.Handler
{
    public class ConsultarSubastasGanadasUsuarioHandler : IRequestHandler<ConsultarSubastasGanadasUsuarioQuery, List<HistorialSubastasGanadasDTO>>
    {
        private readonly ISubastaService _subastaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProductoService _productoService;
        private readonly IPujaService _pujaService;

        public ConsultarSubastasGanadasUsuarioHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService, IPujaService pujaService    )
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
            _pujaService = pujaService;
        }

        public async Task<List<HistorialSubastasGanadasDTO>> Handle(ConsultarSubastasGanadasUsuarioQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Guid idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.SubastaDto.correoUsuario);

                var subastasGanadas = await _subastaService.ObtenerSubastasGanadasDetalleMongoAsync(idUsuario);


                if (subastasGanadas == null || !subastasGanadas.Any())
                {
                    return new List<HistorialSubastasGanadasDTO>();
                }
                var listaSubastasGanadasProducto = new List<HistorialSubastasGanadasDTO>();

                foreach (var subasta in subastasGanadas)
                {

                    var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

                    var pujaGanadora = await _pujaService.ObtenerPujaGanadoraPorIdSubasta(subasta.Id);

                    listaSubastasGanadasProducto.Add(new HistorialSubastasGanadasDTO
                    {
                        IdSubasta = subasta.Id,
                        NombreSubasta = subasta.nombreSubasta.Nombre,
                        DescripcionSubasta = subasta.descripcionSubasta.descripcion,
                        Estado = subasta.estadoSubasta.estado,
                        FechaInicio = subasta.fechaInicioSubasta.fechaInicio,
                        FechaFin = subasta.fechaFinSubasta.fechaFin,
                        incrementoMinimo = subasta.incrementoMinimoSubasta.incrementoMinimo,
                        precioReserva = subasta.precioReservaSubasta.precioReserva,
                        montoGanador = pujaGanadora.MontoPuja.montoPuja,
                        IdProducto = producto.Id,
                        NombreProducto = producto.NombreProducto.Nombre,
                        DescripcionProducto = producto.DescripcionProducto.descripcion,
                        PrecioBase = producto.PrecioBaseProducto.precio,
                        Categoria = producto.CategoriaProducto.categoria,
                        urlImagen = producto.ImagenURLProducto.url

                    });
                }

                return listaSubastasGanadasProducto;
            }
            catch (Exception ex)
            {
                throw new FalloAlObtenerSubastasException("Ocurrió un error al obtener la subastas ganadoras de la base de datos", ex);
            }
        }
    }
}
