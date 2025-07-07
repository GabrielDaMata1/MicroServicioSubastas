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
    /// <summary>
    /// Clase Service que se encarga de procesar todas las operaciones sobre un producto, realizando peticiones HTTP al Microservicio Producto.
    /// </summary>
    public class PujaService: IPujaService
    {
        /// <summary>
        /// Atributo que se encarga de procesar las solicitudes a servicios externos.
        /// </summary>
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;

        public PujaService(HttpClient httpClient, IUsuarioService usuarioService)
        {
            _httpClient = httpClient;
            _usuarioService = usuarioService;
        }
        /// <summary>
        /// Método que se encarga de obtener la puja ganadora de una subtasta por su ID en el Microservicio Pujas.
        /// </summary>
        /// <param name="idSubasta">Parametro que corresponde al ID de la subasta a consultar</param>
        /// <returns>Retorna un objeto Puja con su detalle. Si no lo consigue, retorna null</returns>
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

        /// <summary>
        /// Método que se encarga de obtener las pujas de una subtasta por su ID en el Microservicio Pujas.
        /// </summary>
        /// <param name="idSubasta">Parametro que corresponde al ID de la subasta a consultar</param>
        /// <returns>Retorna una lista de objetos Puja con su detalle. Si no lo consigue, retorna null</returns>
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
