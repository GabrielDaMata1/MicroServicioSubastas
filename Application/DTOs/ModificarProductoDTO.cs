using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ModificarProductoDTO
{
    public Guid Id { get; set; }
    public string NombreProducto { get; set; }
    public string DescripcionProducto { get; set; }
    public string ImagenURLProducto { get; set; }

    public decimal PrecioBaseProducto { get; set; }

    public string CategoriaProducto { get; set; }

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
