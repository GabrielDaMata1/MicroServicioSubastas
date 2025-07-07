using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    /// <summary>
    /// Clase Entity que representa a la entidad Producto en el dominio del sistema.
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Atributo que corresponde al ID del producto.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre del producto.
        /// </summary>
        public NombreProductoVO NombreProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripción del producto.
        /// </summary>
        public DescripcionProductoVO DescripcionProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la imagen url del producto .
        /// </summary>
        public ImagenURLProductoVO ImagenURLProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio base del producto.
        /// </summary>
        public PrecioBaseProductoVO PrecioBaseProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la categoria del producto.
        /// </summary>
        public CategoriaProductoVO CategoriaProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado del producto.
        /// </summary>
        public EstadoProductoVO EstadoProducto { get; set; }

        public Producto () { }
        [JsonConstructor]
        public Producto(NombreProductoVO nombreProducto, DescripcionProductoVO descripcionProducto, ImagenURLProductoVO imagenUrlProducto, PrecioBaseProductoVO precioBaseProducto)
        {
            Id = Guid.NewGuid();
            NombreProducto = nombreProducto;
            DescripcionProducto = descripcionProducto;
            ImagenURLProducto = imagenUrlProducto;
            PrecioBaseProducto = precioBaseProducto;
            EstadoProducto = new EstadoProductoVO("Disponible");

        }

        public Producto(Guid id, NombreProductoVO nombreProducto, DescripcionProductoVO descripcionProducto, ImagenURLProductoVO imagenUrlProducto, PrecioBaseProductoVO precioBaseProducto, CategoriaProductoVO categoriaProducto, EstadoProductoVO estadoProducto)
        {
            Id = id;
            NombreProducto = nombreProducto;
            DescripcionProducto = descripcionProducto;
            ImagenURLProducto = imagenUrlProducto;
            PrecioBaseProducto = precioBaseProducto;
            CategoriaProducto= categoriaProducto;
            EstadoProducto= estadoProducto;
        }

        public Producto(Guid id,NombreProductoVO nombreProducto, DescripcionProductoVO descripcionProducto, ImagenURLProductoVO imagenUrlProducto, PrecioBaseProductoVO precioBaseProducto, EstadoProductoVO estadoProducto)
        {
            Id = id;
            NombreProducto = nombreProducto;
            DescripcionProducto = descripcionProducto;
            ImagenURLProducto = imagenUrlProducto;
            PrecioBaseProducto = precioBaseProducto;
            EstadoProducto = estadoProducto;

        }

        public Producto(Guid id, NombreProductoVO nombreProducto, DescripcionProductoVO descripcionProducto, PrecioBaseProductoVO precioBaseProducto, CategoriaProductoVO categoriaProducto, EstadoProductoVO estadoProducto)
        {
            Id = id;
            NombreProducto = nombreProducto;
            DescripcionProducto = descripcionProducto;
            PrecioBaseProducto = precioBaseProducto;
            CategoriaProducto = categoriaProducto;
            EstadoProducto = estadoProducto;
        }
    }
}
