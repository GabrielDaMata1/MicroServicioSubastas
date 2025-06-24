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
using Domain.Value_Objects;

namespace Application.Service
{
    public class PujaService: IPujaService
    {
        private readonly HttpClient _httpClient;

        public PujaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Puja> ObtenerPujaGanadoraPorIdSubasta(Guid idSubasta)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5004/api/Pujas/obtenerPujaGanadora/{idSubasta}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var contenido = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
                var dto = JsonSerializer.Deserialize<PujaDTO>(contenido, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dto == null)
                {
                    return null;
                }

                var puja = new Puja(
                    dto.id,
                    dto.idUsuario,
                    dto.idSubasta,
                    new MontoPujaVO(dto.montoPuja),
                    new MontoMaximoPujaVO(dto.montoMaximo),
                    new TipoPujaVO(dto.tipoPuja),
                    new MontoPredeterminadoPujaVO(dto.montoPredeterminado)

                );

                return puja;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
