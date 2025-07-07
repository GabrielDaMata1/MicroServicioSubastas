using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre productos, en el Microservicio Producto.
    /// </summary>
    public interface IProductoService
    {
        /// <summary>
        /// Método que se encarga de obtener un producto por su ID en el Microservicio Producto.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna un objeto Producto con su detalle. Si no lo consigue, retorna null</returns>
        Task<Producto> ObtenerProductoPorGuid(Guid idProducto);
        /// <summary>
        /// Método que se encarga de obtener el ID del usuario por el ID del producto en el Microservicio Producto.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna un GUID correspondiente al ID del usuario. Si no lo consigue, retorna  un GUID vacío</returns>
        Task<Guid> ObtenerUsuarioIdPorIdProductoAsync(Guid idProducto);
        /// <summary>
        /// Método que se encarga de modificar un producto en el Microservicio Producto.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del dueño del producto a modificar</param>
        /// <param name="productoModificar">Parametro que corresponde al objeto Producto a modificar</param>
        /// <param name="estado">Parametro que corresponde al nuevo estado del producto a modificar</param>
        /// <returns>Retorna un valor booleano si la operación fue exitosa</returns>
        Task<bool> ModificarProductoAsync(string correo, Producto productoModificar, string estado);
        /// <summary>
        /// Método que se encarga de modificar el estado de un producto a disponible en el Microservicio Producto.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del dueño del producto a modificar</param>
        /// <param name="producto">Parametro que corresponde al objeto Producto a modificar</param>
        /// <returns>Retorna un valor booleano si la operación fue exitosa</returns>
        Task<bool> ModificarProductoDisponibleAsync(string correo, Producto producto);
    }
}
