using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISubastaRepositoryMongo
    {
        Task<HttpStatusCode> RegistrarSubastaAsync(Subasta subasta, Guid IdUsuario);
        Task<HttpStatusCode> ModificarSubastaAsync(Subasta subasta, Guid IdUsuario);

        Task<Subasta> ObtenerSubastaPorId(Guid idSubasta);
    }
}
