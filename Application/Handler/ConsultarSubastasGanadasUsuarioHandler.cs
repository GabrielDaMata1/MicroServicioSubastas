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
    /// Clase Handler que se encarga consultar las subastas ganadas con su puja ganadora por un usuario.
    /// </summary>
    public class ConsultarSubastasGanadasUsuarioHandler : IRequestHandler<ConsultarSubastasGanadasUsuarioQuery, List<HistorialSubastasGanadasDTO>>
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
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre las pujas en el Microservicio Pujas, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IPujaService _pujaService;

        public ConsultarSubastasGanadasUsuarioHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService, IPujaService pujaService    )
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
            _pujaService = pujaService;
        }

        /// <summary>
        /// Metodo que se encarga de procesar la consulta de las subastas ganadas.
        /// </summary>
        /// <param name="request">Parametro DTO que contiene el ID del usuario ganador de las subastas.</param>
        /// <returns>Retorna una lista de DTOs con los datos de la subastas ganadas.</returns>
        /// <exception cref="FalloAlObtenerSubastasException">
        /// Esta excepcion ocurre si no se pudo obtener la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task<List<HistorialSubastasGanadasDTO>> Handle(ConsultarSubastasGanadasUsuarioQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Se obtiene el ID del usuario que ganó las subastas
                Guid idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.SubastaDto.correoUsuario);

                //Se obtiene la lista de subastas ganadas por un usuario en la base de datos en MongoDB
                var subastasGanadas = await _subastaService.ObtenerSubastasGanadasDetalleMongoAsync(idUsuario);


                //En caso de que la consulta retorne null, se devuelve una lista vacía
                if (subastasGanadas == null || !subastasGanadas.Any())
                {
                    return new List<HistorialSubastasGanadasDTO>();
                }
                var listaSubastasGanadasProducto = new List<HistorialSubastasGanadasDTO>();

                foreach (var subasta in subastasGanadas)
                {
                    //Se obtiene el producto que se está subastando desde el Microservicio de Producto
                    var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

                    //Se obtiene la puja ganadora de la subasta desde el Microservicio de Pujas
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
