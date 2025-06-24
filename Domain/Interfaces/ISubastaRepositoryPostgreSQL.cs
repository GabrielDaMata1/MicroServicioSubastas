using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISubastaRepositoryPostgreSQL
    {
        Task<Guid> RegistrarSubastaAsync(Subasta subasta, Guid IdUsuario);
        Task<HttpStatusCode> ModificarSubastaAsync(Subasta subasta, Guid IdUsuario);

        Task<bool> EliminarSubastaAsync(Guid idSubasta);

        Task<HttpStatusCode> ActualizarEstadoSubasta(Guid idSubasta, string nuevoEstado);
    }
}
