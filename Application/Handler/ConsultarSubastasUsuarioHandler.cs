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
    /// <summary>
    /// Clase Handler que se encarga consultar todas las subastas realizadas por un subastador.
    /// </summary>
    public class ConsultarSubastasUsuarioHandler : IRequestHandler<ConsultarSubastasUsuarioQuery, List<HistorialSubastasDTO>>
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

        public ConsultarSubastasUsuarioHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService)
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
        }
        /// <summary>
        /// Metodo que se encarga de procesar la consulta de todas las subastas organizadas por un subastador.
        /// </summary>
        /// <param name="request">Parametro DTO que contiene el correo del subastador a consultar.</param>
        /// <returns>Retorna una lista de DTOs con los datos de la subasta.</returns>
        /// <exception cref="FalloAlObtenerSubastasException">
        /// Esta excepcion ocurre si no se pudo obtener las subastas en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task<List<HistorialSubastasDTO>> Handle(ConsultarSubastasUsuarioQuery request, CancellationToken cancellationToken)
        {
            try
            {

                //Se obtiene el ID del subastador que organizó las subastas desde el Microservicio Usuarios
                Guid idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.SubastaDto.correoUsuario);

                //Se obtienen las subastas organizadas por el subastador desde la base de datos en MongoDB
                var subastas = await _subastaService.ObtenerSubastasPorUsuarioMongoAsync(idUsuario);

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
