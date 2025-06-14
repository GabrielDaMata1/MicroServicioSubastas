using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exceptions;
using Application.Querys;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    public class ConsultarSubastaHandler : IRequestHandler<ConsultarSubastaQuery, HistorialSubastasDTO>
    {
        private readonly ISubastaService _subastaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProductoService _productoService;

        public ConsultarSubastaHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService)
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
        }

        public async Task<HistorialSubastasDTO> Handle(ConsultarSubastaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subasta = await _subastaService.ObtenerSubastaMongoAsync(request.SubastaDto.idSubasta);

                if (subasta == null)
                {
                    return new HistorialSubastasDTO();
                }
                
                var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

                var SubastaConProducto = new HistorialSubastasDTO
                {
                    IdSubasta = subasta.Id,
                    NombreSubasta = subasta.nombreSubasta.Nombre,
                    DescripcionSubasta = subasta.descripcionSubasta.descripcion,
                    Estado = subasta.estadoSubasta.estado,
                    FechaInicio = subasta.fechaInicioSubasta.fechaInicio,
                    FechaFin = subasta.fechaFinSubasta.fechaFin,
                    incrementoMinimo = subasta.incrementoMinimoSubasta.incrementoMinimo,
                    precioReserva = subasta.precioReservaSubasta.precioReserva,
                    IdProducto = producto.Id,
                    NombreProducto = producto.NombreProducto.Nombre,
                    DescripcionProducto = producto.DescripcionProducto.descripcion,
                    PrecioBase = producto.PrecioBaseProducto.precio,
                    Categoria = producto.CategoriaProducto.categoria
                };
                return SubastaConProducto;
            }
            catch (Exception ex)
            {
                throw new FalloAlObtenerSubastasException("Ocurrió un error al obtener la subasta de la base de datos", ex);
            }
        }
    }
}
