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
    /// <summary>
    /// Clase Handler que se encarga consultar las subastas ganadas con sus pujas.
    /// </summary>
    public class ConsultarSubastasGanadasPujasHandler : IRequestHandler<ConsultarSubastasGanadasPujasQuery, List<HistorialPujasUsuarioDTO>>
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

        public ConsultarSubastasGanadasPujasHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService, IPujaService pujaService)
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
            _pujaService = pujaService;
        }

        /// <summary>
        /// Metodo que se encarga de procesar la consulta de las subastas ganadas con sus pujas.
        /// </summary>
        /// <returns>Retorna una lista de DTOs con los datos de la subastas ganadas y el detalle de sus pujas.</returns>
        /// <exception cref="FalloAlObtenerSubastasException">
        /// Esta excepcion ocurre si no se pudo obtener la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task<List<HistorialPujasUsuarioDTO>> Handle(ConsultarSubastasGanadasPujasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Se obtienen las subastas ganadas en la base de datos en MongoDB
                var subastas = await _subastaService.ObtenerSubastasGanadasMongoAsync();

                var resultado = new List<HistorialPujasUsuarioDTO>();

                foreach (var subasta in subastas)
                {
                    var subastaId = subasta.Id;
                    //Se obtiene el producto de la subasta desde el Microservicio Producto
                    var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

                    //Se obtiene la lista de pujas de la subasta desde el Microservicio Pujas
                    var pujasSubasta = await _pujaService.ObtenerPujasSubasta(subastaId);

                    var tareasPujas = pujasSubasta.Select(async p =>
                    {
                        //Se obtiene el correo del usuario que realizó la puja en la subasta desde el Microservicio de Usuarios
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
