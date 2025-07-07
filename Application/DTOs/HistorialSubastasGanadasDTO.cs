using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para mostrar el historial de subastas ganadas con su detalle
    /// </summary>
    public class HistorialSubastasGanadasDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la subasta.
        /// </summary>
        public Guid IdSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre de la subasta.
        /// </summary>
        public string NombreSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripcion de la subasta.
        /// </summary>
        public string DescripcionSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta.
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de inicio de la subasta.
        /// </summary>
        public DateTime FechaInicio { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha fin de la subasta.
        /// </summary>
        public DateTime FechaFin { get; set; }
        /// <summary>
        /// Atributo que corresponde al incremento mínimo de la subasta.
        /// </summary>
        public decimal incrementoMinimo { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio de reserva de la subasta.
        /// </summary>
        public decimal precioReserva { get; set; }
        /// <summary>
        /// Atributo que corresponde al monto final de la subasta.
        /// </summary>
        public decimal montoGanador { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del producto subastado en de la subasta.
        /// </summary>
        public Guid IdProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre del producto subastado en de la subasta.
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripción del producto subastado en de la subasta.
        /// </summary>
        public string DescripcionProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio base del producto subastado en de la subasta.
        /// </summary>
        public decimal PrecioBase { get; set; }
        /// <summary>
        /// Atributo que corresponde a la categoría del producto subastado en de la subasta.
        /// </summary>
        public string Categoria { get; set; }
        /// <summary>
        /// Atributo que corresponde a la dirección URL de la imagen del producto subastado en de la subasta en Firebase.
        /// </summary>
        public string urlImagen { get; set; }
    }
}
