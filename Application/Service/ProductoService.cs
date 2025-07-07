using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Application.Service
{
    /// <summary>
    /// Clase Service que se encarga de procesar todas las operaciones sobre un producto, realizando peticiones HTTP al Microservicio Producto.
    /// </summary>
    public class ProductoService: IProductoService
    {
        /// <summary>
        /// Atributo que se encarga de procesar las solicitudes a servicios externos.
        /// </summary>
        private readonly HttpClient _httpClient;

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /// <summary>
        /// Método que se encarga de obtener un producto por su ID en el Microservicio Producto.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna un objeto Producto con su detalle. Si no lo consigue, retorna null</returns>
        public async Task<Producto> ObtenerProductoPorGuid(Guid idProducto)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5002/api/Productos/consultarProducto/{idProducto}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var contenido = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
                var dto = JsonSerializer.Deserialize<ProductoDTO>(contenido, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dto == null)
                {
                    return null;
                }

                var producto = new Producto(
                    dto.Id,
                    new NombreProductoVO(dto.NombreProducto),
                    new DescripcionProductoVO(dto.DescripcionProducto),
                    new ImagenURLProductoVO(dto.ImagenURLProducto),
                    new PrecioBaseProductoVO(dto.PrecioBaseProducto),
                    new CategoriaProductoVO(dto.CategoriaProducto),
                    new EstadoProductoVO(dto.EstadoProducto)

                );

                return producto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Método que se encarga de obtener el ID del usuario por el ID del producto en el Microservicio Producto.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna un GUID correspondiente al ID del usuario. Si no lo consigue, retorna  un GUID vacío</returns>
        public async Task<Guid> ObtenerUsuarioIdPorIdProductoAsync(Guid idProducto)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5002/api/Productos/consultarIdUsuarioProducto/{idProducto}");

            if (!response.IsSuccessStatusCode)
            {
                return Guid.Empty;
            }

            var guidString = await response.Content.ReadAsStringAsync();
            if (Guid.TryParse(guidString.Trim('"'), out Guid userId))
            {
                return userId;
            }
            else
            {
                return Guid.Empty;

            }
        }
        /// <summary>
        /// Método que se encarga de modificar un producto en el Microservicio Producto.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del dueño del producto a modificar</param>
        /// <param name="producto">Parametro que corresponde al objeto Producto a modificar</param>
        /// <param name="estado">Parametro que corresponde al nuevo estado del producto a modificar</param>
        /// <returns>Retorna un valor booleano si la operación fue exitosa</returns>
        public async Task<bool> ModificarProductoAsync(string correo, Producto producto, string estado)
        {

            var productoModificarDto= new ModificarProductoDTO(producto.Id, producto.NombreProducto.Nombre,
                producto.DescripcionProducto.descripcion, producto.ImagenURLProducto.url,
                producto.PrecioBaseProducto.precio, producto.CategoriaProducto.categoria, estado);
            var requestUri = $"http://localhost:5002/api/Productos/modificarProducto/{correo}";

            var contenidoJson = JsonSerializer.Serialize(productoModificarDto);
            var contenido = new StringContent(contenidoJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUri, contenido);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Método que se encarga de modificar el estado de un producto a disponible en el Microservicio Producto.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del dueño del producto a modificar</param>
        /// <param name="producto">Parametro que corresponde al objeto Producto a modificar</param>
        /// <returns>Retorna un valor booleano si la operación fue exitosa</returns>
        public async Task<bool> ModificarProductoDisponibleAsync(string correo, Producto producto)
        {

            var productoModificarDto = new ModificarProductoDTO(producto.Id, producto.NombreProducto.Nombre,
                producto.DescripcionProducto.descripcion, producto.ImagenURLProducto.url,
                producto.PrecioBaseProducto.precio, producto.CategoriaProducto.categoria, "Disponible");
            var requestUri = $"http://localhost:5002/api/Productos/modificarProducto/{correo}";

            var contenidoJson = JsonSerializer.Serialize(productoModificarDto);
            var contenido = new StringContent(contenidoJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUri, contenido);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }


    }
}
