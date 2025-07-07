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
    /// <summary>
    /// Clase Handler que se encarga consultar una subasta en específico de un subastador.
    /// </summary>
    public class ConsultarSubastaHandler : IRequestHandler<ConsultarSubastaQuery, HistorialSubastasDTO>
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

        public ConsultarSubastaHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService)
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
        }
        /// <summary>
        /// Metodo que se encarga de procesar la consulta de una subasta organizada por un subastador.
        /// </summary>
        /// <param name="request">Parametro DTO que contiene el ID de la subasta a consultar.</param>
        /// <returns>Retorna un DTO con los datos de la subasta.</returns>
        /// <exception cref="FalloAlObtenerSubastasException">
        /// Esta excepcion ocurre si no se pudo obtener la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task<HistorialSubastasDTO> Handle(ConsultarSubastaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Se obtiene la subasta por el ID dado
                var subasta = await _subastaService.ObtenerSubastaMongoAsync(request.SubastaDto.idSubasta);

                //En caso de que la consulta retorne null, se devuelve un DTO vacío
                if (subasta == null)
                {
                    return new HistorialSubastasDTO();
                }

                //Se obtiene el producto que se está subastando desde el Microservicio Producto
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
                    idUsuario = subasta.idUsuario,
                    IdProducto = producto.Id,
                    NombreProducto = producto.NombreProducto.Nombre,
                    DescripcionProducto = producto.DescripcionProducto.descripcion,
                    PrecioBase = producto.PrecioBaseProducto.precio,
                    Categoria = producto.CategoriaProducto.categoria,
                    urlImagen = producto.ImagenURLProducto.url
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
