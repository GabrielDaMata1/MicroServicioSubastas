using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exceptions;
using Application.Querys;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handler
{
    public class ConsultarSubastasGanadasPujasHandler : IRequestHandler<ConsultarSubastasGanadasPujasQuery, List<HistorialPujasUsuarioDTO>>
    {
        private readonly ISubastaService _subastaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProductoService _productoService;
        private readonly IPujaService _pujaService;

        public ConsultarSubastasGanadasPujasHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService, IPujaService pujaService)
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
            _pujaService = pujaService;
        }

        public async Task<List<HistorialPujasUsuarioDTO>> Handle(ConsultarSubastasGanadasPujasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subastas = await _subastaService.ObtenerSubastasGanadasMongoAsync();

                var resultado = new List<HistorialPujasUsuarioDTO>();

                foreach (var subasta in subastas)
                {
                    var subastaId = subasta.Id;
                    var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);
                    var pujasSubasta = await _pujaService.ObtenerPujasSubasta(subastaId);

                    var tareasPujas = pujasSubasta.Select(async p =>
                    {
                        var correo = await _usuarioService.ObtenerCorreoPorIdAsync(p.IdUsuario);
                        return new PujaUsuarioDTO
                        {
                            id = p.Id,
                            montoPuja = p.MontoPuja.montoPuja,
                            idSubasta = subastaId,
                            correoUsuario = correo,
                            tipoPuja = p.TipoPuja.tipoPuja,
                            montoMaximo = p.MontoMaximo.montoMaximo,
                            montoPredeterminado = p.MontoPredeterminado.montoPredeterminado,
                            fecha = p.FechaPuja.fechaPuja,
                        };
                    });

                    var pujasDTO = await Task.WhenAll(tareasPujas);

                    var dto = new HistorialPujasUsuarioDTO
                    {
                        IdSubasta = subastaId,
                        IdUsuario = subasta.idUsuario,
                        NombreSubasta = subasta.nombreSubasta.Nombre,
                        DescripcionSubasta = subasta.descripcionSubasta.descripcion,
                        Estado = subasta.estadoSubasta.estado,
                        FechaInicio = subasta.fechaInicioSubasta.fechaInicio,
                        FechaFin = subasta.fechaFinSubasta.fechaFin,
                        IncrementoMinimo = subasta.incrementoMinimoSubasta.incrementoMinimo,
                        PrecioReserva = subasta.precioReservaSubasta.precioReserva,
                        IdProducto = producto.Id,
                        NombreProducto = producto.NombreProducto.Nombre,
                        DescripcionProducto = producto.DescripcionProducto.descripcion,
                        PrecioBase = producto.PrecioBaseProducto.precio,
                        Categoria = producto.CategoriaProducto.categoria,
                        UrlImagen = producto.ImagenURLProducto.url,
                        Pujas = pujasDTO.ToList()
                    };

                    resultado.Add(dto);
                }

                return resultado;

            }
            catch (Exception ex)
            {
                throw new FalloAlObtenerSubastasException("Ocurrió un error al obtener las subastas de la base de datos", ex);
            }
        }
    }
}
