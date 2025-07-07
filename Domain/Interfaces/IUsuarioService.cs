using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre usuarios, en el Microservicio Usuarios.
    /// </summary>
    public interface IUsuarioService
    {
        /// <summary>
        /// Método que se encarga de obtener el ID de un usuario por su correo en el Microservicio Usuarios.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del usuario a consultar</param>
        /// <returns>Retorna un valor GUID que corresponde al ID del usuario consultado.
        /// Si no lo consigue, retorna un GUID vacio</returns>
        Task<Guid> ObtenerUsuarioPorIdAsync(string correo);
        /// <summary>
        /// Método que se encarga de obtener el correo de un usuario por su ID en el Microservicio Usuarios.
        /// </summary>
        /// <param name="idUsuario">Parametro que corresponde al ID del usuario a consultar</param>
        /// <returns>Retorna un valor string que corresponde al correo del usuario consultado.
        /// Si no lo consigue, retorna null</returns>
        Task<string> ObtenerCorreoPorIdAsync(Guid idUsuario);
    }
}
