using System.Net.Http;
using System.Threading.Tasks;
using Domain.Interfaces;

public class UsuarioService: IUsuarioService
{
    private readonly HttpClient _httpClient;

    public UsuarioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Guid> ObtenerUsuarioPorIdAsync(string correo)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5001/api/usuarios/IdUsuario/{correo}");

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

