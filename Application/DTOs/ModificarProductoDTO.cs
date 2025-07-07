using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para modificar un producto
    /// </summary>
   public class ModificarProductoDTO
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
        /// Atributo que corresponde a la descripcion del producto.
        /// </summary>
        public string DescripcionProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la dirección URL de la imagen del producto  en la en Firebase.
        /// </summary>
        public string ImagenURLProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio base del producto.
        /// </summary>
        public decimal PrecioBaseProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la categoría del producto 
        /// </summary>
        public string CategoriaProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado del producto.
        /// </summary>
        public string EstadoProducto { get; set; }

        public ModificarProductoDTO(Guid id, string nombreProducto, string descripcionProducto, string imagenUrl, decimal precioBase, string categoriaProducto, string estadoProducto) { 
            Id = id;
            NombreProducto = nombreProducto;
            DescripcionProducto = descripcionProducto;
            ImagenURLProducto = imagenUrl;
            PrecioBaseProducto = precioBase;
            CategoriaProducto = categoriaProducto;
            EstadoProducto = estadoProducto;
        }
}
}
