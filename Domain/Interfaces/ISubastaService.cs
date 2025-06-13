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

        Task<HttpStatusCode> RegistrarProductoMongoAsync(Subasta subasta, Guid IdUsuario);
    }
}
