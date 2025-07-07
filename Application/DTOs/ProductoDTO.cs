using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para obtener el producto desde el Microservicio Producto
    /// </summary>
    public class ProductoDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID del producto.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre del producto.
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripción del producto.
        /// </summary>
        public string DescripcionProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio base del producto.
        /// </summary>
        public decimal PrecioBaseProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la categoría del producto.
        /// </summary>
        public string CategoriaProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado del producto.
        /// </summary>
        public string EstadoProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la dirección url de la imagen del producto en Firebase.
        /// </summary>
        public string ImagenURLProducto { get; set; }
    }
}
