using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISubastaService
    {
        Task<Guid> RegistrarSubastaPostgreSQLAsync(Subasta subasta, Guid IdUsuario);

        Task<HttpStatusCode> RegistrarSubastaMongoAsync(Subasta subasta, Guid IdUsuario);

        Task<HttpStatusCode> ModificarSubastaPostgreSQLAsync(Subasta subasta, Guid IdUsuario);
        Task<HttpStatusCode> ModificarSubastaMongoAsync(Subasta subasta, Guid IdUsuario);

        Task<Subasta> ObtenerSubastaPorIdMongoAsync(Guid idSubasta);

        Task<bool> EliminarSubastaMongoAsync(Guid idSubasta);

        Task<bool> EliminarSubastaPostgreSQLAsync(Guid idSubasta);

        Task<Guid> ObtenerUsuarioIdPorSubastaIdMongoAsync(Guid idSubasta);
    }
}
