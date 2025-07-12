using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Service
{
    public class NotificacionService: INotificacionService
    {
        private readonly HttpClient _httpClient;

        public NotificacionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> EnviarNotificacionUsuarioGanadorSubasta(string destinatario, decimal montoGanador, string nombreSubasta, string nombreProducto)
        {
            var payload = new
            {
                destinatario,
                montoGanador,
                nombreSubasta,
                nombreProducto
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5287/api/Notification/enviarNotificacionUsuarioGanadorSubasta", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> EnviarNotificacionSubastadorSubastaFinalizada(string destinatario, decimal montoGanador, string nombreSubasta, string nombreProducto, string correoGanador)
        {
            var payload = new
            {
                destinatario,
                montoGanador,
                nombreSubasta,
                nombreProducto,
                correoGanador
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5287/api/Notification/enviarNotificacionSubastadorSubastaFinalizada", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> EnviarCorreoUsuariosSubastaCancelada(string correoSubastador, string correoUsuario, string nombreSubasta, string nombreProducto)
        {
            var payload = new
            {
                correoSubastador,
                correoUsuario,
                nombreSubasta,
                nombreProducto,
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5287/api/Notification/enviarCorreoUsuariosSubastaCancelada", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
