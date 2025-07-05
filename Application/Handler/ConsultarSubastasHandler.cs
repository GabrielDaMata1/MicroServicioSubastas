using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exceptions;
using Application.Querys;
using Application.Service;
using Domain.Entities;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    public class ConsultarSubastasHandler : IRequestHandler<ConsultarSubastasQuery, List<HistorialSubastasDTO>>
    {
        private readonly ISubastaService _subastaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProductoService _productoService;

        public ConsultarSubastasHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService)
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService= productoService;
        }

        public async Task<List<HistorialSubastasDTO>> Handle(ConsultarSubastasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subastas = await _subastaService.ObtenerSubastasMongo();

                if (subastas == null || !subastas.Any())
                {
                    return new List<HistorialSubastasDTO>();
                }
                var listaSubastaProducto = new List<HistorialSubastasDTO>();

                foreach (var subasta in subastas)
                {
                    var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);


                    listaSubastaProducto.Add(new HistorialSubastasDTO
                    {
                        IdSubasta = subasta.Id,
                        NombreSubasta = subasta.nombreSubasta.Nombre,
                        DescripcionSubasta = subasta.descripcionSubasta.descripcion,
                        Estado = subasta.estadoSubasta.estado,
                        FechaInicio = subasta.fechaInicioSubasta.fechaInicio,
                        FechaFin = subasta.fechaFinSubasta.fechaFin,
                        incrementoMinimo = subasta.incrementoMinimoSubasta.incrementoMinimo,
                        precioReserva = subasta.precioReservaSubasta.precioReserva,
                        idUsuario = subasta.idUsuario,
                        IdProducto = producto.Id,
                        NombreProducto = producto.NombreProducto.Nombre,
                        DescripcionProducto = producto.DescripcionProducto.descripcion,
                        PrecioBase = producto.PrecioBaseProducto.precio,
                        Categoria = producto.CategoriaProducto.categoria,
                        urlImagen = producto.ImagenURLProducto.url

                    });
                }

                return listaSubastaProducto;
            }
            catch (Exception ex)
            {
                throw new FalloAlObtenerSubastasException("Ocurrió un error al obtener la subastas de la base de datos", ex);
            }
        }
    }
}
