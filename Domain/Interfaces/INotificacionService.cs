using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre las notificaciones, en el Microservicio Producto.
    /// </summary>
    public interface INotificacionService
    {
        Task<bool> EnviarNotificacionUsuarioGanadorSubasta(string destinatario, decimal montoGanador, string nombreSubasta, string nombreProducto);

        Task<bool> EnviarNotificacionSubastadorSubastaFinalizada(string destinatario, decimal montoGanador, string nombreSubasta, string nombreProducto, string correoGanador);

        Task<bool> EnviarCorreoUsuariosSubastaCancelada(string correoSubastador, string correoUsuario,string nombreSubasta, string nombreProducto);


    }
}
