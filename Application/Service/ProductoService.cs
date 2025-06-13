using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Service
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Producto> ObtenerProductoPorGuid(Guid idProducto)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5001/api/ProductosconsultarProducto/{idProducto}");

            if (!response.IsSuccessStatusCode)
            {
                return new Producto();
            }

            var contenido = await response.Content.ReadAsStringAsync();

            var producto = JsonSerializer.Deserialize<Producto>(contenido, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return producto;

        }
    }
}
