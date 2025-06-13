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
        Task<HttpStatusCode> RegistrarProductoAsync(Subasta subasta, Guid IdUsuario);
    }
}
