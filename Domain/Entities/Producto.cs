using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    public class Producto
    {
        public Guid Id { get; set; }
        public NombreProductoVO NombreProducto { get; set; }
        public DescripcionProductoVO DescripcionProducto { get; set; }
        public ImagenURLProductoVO ImagenURLProducto { get; set; }

        public PrecioBaseProductoVO PrecioBaseProducto { get; set; }

        public CategoriaProductoVO CategoriaProducto { get; set; }

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
