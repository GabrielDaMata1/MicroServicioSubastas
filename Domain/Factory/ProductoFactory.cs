using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;

namespace Domain.Factory
{
    public static class ProductoFactory
    {
        public static Producto CrearProducto(string nombre, string descripcion, string urlImagen, decimal precioBase)
        {
            var nombreVO = new NombreProductoVO(nombre);
            var descripcionVO = new DescripcionProductoVO(descripcion);
            var urlImagenVO = new ImagenURLProductoVO(urlImagen);
            var precioBaseVO = new PrecioBaseProductoVO(precioBase);

            return new Producto(nombreVO, descripcionVO, urlImagenVO, precioBaseVO);
        }

        public static Producto CrearProductoConId(Guid id, string nombre, string descripcion, string urlImagen, decimal precioBase)
        {
            var nombreVO = new NombreProductoVO(nombre);
            var descripcionVO = new DescripcionProductoVO(descripcion);
            var urlImagenVO = new ImagenURLProductoVO(urlImagen);
            var precioBaseVO = new PrecioBaseProductoVO(precioBase);

            return new Producto(id,nombreVO, descripcionVO, urlImagenVO, precioBaseVO);
        }

        public static Producto CrearProductoConIdYCategoria(Guid id, string nombre, string descripcion, string urlImagen, decimal precioBase, string categoria)
        {
            var nombreVO = new NombreProductoVO(nombre);
            var descripcionVO = new DescripcionProductoVO(descripcion);
            var urlImagenVO = new ImagenURLProductoVO(urlImagen);
            var precioBaseVO = new PrecioBaseProductoVO(precioBase);
            var categoriaVO = new CategoriaProductoVO(categoria);

            return new Producto(id, nombreVO, descripcionVO, urlImagenVO, precioBaseVO, categoriaVO);
        }
    }
}
