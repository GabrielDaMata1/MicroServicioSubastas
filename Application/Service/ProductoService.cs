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
    public class ProductoService: IProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
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
                    new CategoriaProductoVO(dto.CategoriaProducto),
                    new PrecioBaseProductoVO(dto.PrecioBaseProducto)
                    
                );

                return producto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

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
    }
}
