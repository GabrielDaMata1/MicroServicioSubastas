using System.Net.Http;
using System.Threading.Tasks;
using Domain.Interfaces;

/// <summary>
/// Clase Service que se encarga de procesar todas las operaciones sobre un usuario, realizando peticiones HTTP al Microservicio Usuarios.
/// </summary>
public class UsuarioService: IUsuarioService
{
    /// <summary>
    /// Atributo que se encarga de procesar las solicitudes a servicios externos.
    /// </summary>
    private readonly HttpClient _httpClient;

    public UsuarioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    /// <summary>
    /// Método que se encarga de obtener el ID de un usuario por su correo en el Microservicio Usuarios.
    /// </summary>
    /// <param name="correo">Parametro que corresponde al correo del usuario a consultar</param>
    /// <returns>Retorna un valor GUID que corresponde al ID del usuario consultado.
    /// Si no lo consigue, retorna un GUID vacio</returns>
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
    /// <summary>
    /// Método que se encarga de obtener el correo de un usuario por su ID en el Microservicio Usuarios.
    /// </summary>
    /// <param name="idUsuario">Parametro que corresponde al ID del usuario a consultar</param>
    /// <returns>Retorna un valor string que corresponde al correo del usuario consultado.
    /// Si no lo consigue, retorna null</returns>
    public async Task<string> ObtenerCorreoPorIdAsync(Guid idUsuario)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5001/api/usuarios/Correo/{idUsuario}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var correo = await response.Content.ReadAsStringAsync();

        return correo;
    }




}

