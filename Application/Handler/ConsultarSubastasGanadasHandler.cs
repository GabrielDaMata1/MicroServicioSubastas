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
    /// Clase Handler que se encarga consultar las subastas ganadas.
    /// </summary>
    public class ConsultarSubastasGanadasHandler : IRequestHandler<ConsultarSubastasGanadasQuery, List<ConsultarSubastasGanadasDTO>>
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

        public ConsultarSubastasGanadasHandler(ISubastaService subastaService, IUsuarioService usuarioService, IProductoService productoService, IPujaService pujaService)
        {
            _subastaService = subastaService;
            _usuarioService = usuarioService;
            _productoService = productoService;
            _pujaService = pujaService;
        }

        /// <summary>
        /// Metodo que se encarga de procesar la consulta de las subastas ganadas.
        /// </summary>
        /// <returns>Retorna una lista de DTOs con los datos de la subastas ganadas.</returns>
        /// <exception cref="FalloAlObtenerSubastasException">
        /// Esta excepcion ocurre si no se pudo obtener la subasta en la base de datos de MongoDB o si ocurre un error inesperado.
        /// </exception>
        public async Task<List<ConsultarSubastasGanadasDTO>> Handle(ConsultarSubastasGanadasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Se obtiene la lista de subastas ganadas en la base de datos en MongoDB
                var subastasGanadas = await _subastaService.ObtenerSubastasGanadasMongoAsync();

                //En caso de que la consulta retorne null, se devuelve un DTO vacío
                if (subastasGanadas == null || !subastasGanadas.Any())
                {
                    return new List<ConsultarSubastasGanadasDTO>();
                }
                var listaSubastasGanadasProducto = new List<ConsultarSubastasGanadasDTO>();

                foreach (var subasta in subastasGanadas)
                {
                    //Se obtiene el producto que se está subastando desde el Microservicio de Producto
                    var producto = await _productoService.ObtenerProductoPorGuid(subasta.idProductoSubasta);

                    //Se obtiene la puja ganadora de la subasta desde el Microservicio de Pujas
                    var pujaGanadora = await _pujaService.ObtenerPujaGanadoraPorIdSubasta(subasta.Id);

                    //Se obtiene el historial de subastas para conocer el correo del ganador de la subasta 
                    var historial = await _subastaService.ObtenerHistorialSubastaMongoAsync(subasta.Id);

                    //Se obtiene el correo del ganador de la subasta desde el Microservicio de Usuarios
                    var correo = await _usuarioService.ObtenerCorreoPorIdAsync(historial.IdUsuario);

                    listaSubastasGanadasProducto.Add(new ConsultarSubastasGanadasDTO
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
                        correoUsuario= correo,
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
                throw new FalloAlObtenerSubastasException("Ocurrió un error al obtener la subastas ganadas de la base de datos", ex);
            }
        }
    }
}
