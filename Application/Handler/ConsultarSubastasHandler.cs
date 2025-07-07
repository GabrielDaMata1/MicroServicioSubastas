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
    /// <summary>
    /// Clase Handler que se encarga consultar todas las subastas.
    /// </summary>
    public class ConsultarSubastasHandler : IRequestHandler<ConsultarSubastasQuery, List<HistorialSubastasDTO>>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto en el Microservicio Producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;

        public ConsultarSubastasHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService)
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService= productoService;
        }

        /// <summary>
        /// Metodo que se encarga de procesar la consultas de todas las subastas.
        /// </summary>
        /// <returns>Retorna una lista de DTOs con los datos de las subastas .</returns>
        /// <exception cref="FalloAlObtenerSubastasException">
        /// Esta excepcion ocurre si no se pudo obtener la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task<List<HistorialSubastasDTO>> Handle(ConsultarSubastasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Se obtienen todas las subastas en la base de datos en MongoDB
                var subastas = await _subastaService.ObtenerSubastasMongo();

                //En caso de que la consulta retorne null, se devuelve una lista vacía
                if (subastas == null || !subastas.Any())
                {
                    return new List<HistorialSubastasDTO>();
                }
                var listaSubastaProducto = new List<HistorialSubastasDTO>();

                foreach (var subasta in subastas)
                {
                    //Se obtiene el producto que se está subastando desde el Microservicio de Producto
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
