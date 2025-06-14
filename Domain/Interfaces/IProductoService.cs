using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductoService
    {
        Task<Producto> ObtenerProductoPorGuid(Guid idProducto);
        Task<Guid> ObtenerUsuarioIdPorIdProductoAsync(Guid idProducto);
    }
}
