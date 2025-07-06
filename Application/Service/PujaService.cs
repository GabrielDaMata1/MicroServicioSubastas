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
        private readonly IUsuarioService _usuarioService;

        public PujaService(HttpClient httpClient, IUsuarioService usuarioService)
        {
            _httpClient = httpClient;
            _usuarioService = usuarioService;
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

        public async Task<List<Puja>> ObtenerPujasSubasta(Guid idSubasta)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5004/api/Pujas/obtenerPujasSubasta/{idSubasta}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var contenido = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
                var dto = JsonSerializer.Deserialize<List<PujaUsuarioDTO>>(contenido, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dto == null)
                {
                    return null;
                }

                var tareas = dto.Select(async p =>
                {
                    var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(p.correoUsuario);
                    return new Puja(
                        p.id,
                        idUsuario,
                        p.idSubasta,
                        new MontoPujaVO(p.montoPuja),
                        new MontoMaximoPujaVO(p.montoMaximo),
                        new TipoPujaVO(p.tipoPuja),
                        new MontoPredeterminadoPujaVO(p.montoPredeterminado),
                        new FechaPujaVO(p.fecha)
                    );
                });

                var pujas = await Task.WhenAll(tareas);
                return pujas.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
