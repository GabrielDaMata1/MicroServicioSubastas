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
        Task<List<Subasta>> ObtenerSubastasMongo();

        Task<Subasta> ObtenerSubastaMongoAsync(Guid idSubasta);

        Task<List<Subasta>> ObtenerSubastasPorUsuarioMongoAsync(Guid idUsuario);

        Task<HttpStatusCode> ActualizarEstadoSubastaMongoAsync(Guid idSubasta, string nuevoEstado);

        Task<HttpStatusCode> ActualizarEstadoSubastaPostgreSQLAsync(Guid idSubasta, string nuevoEstado);

        Task<HttpStatusCode> RegistrarHistorialSubastaMongoAsync(HistorialSubasta historialSubasta, string resultado);

        Task<Guid> RegistrarHistorialSubastaPostgreSQLAsync(HistorialSubasta historialSubasta, string resultado);

        Task<List<HistorialSubasta>> ObtenerSubastasGanadasPorUsuarioMongoAsync(Guid idUsuario);
        Task<List<Subasta>> ObtenerSubastasGanadasDetalleMongoAsync(Guid idUsuario);

        Task<List<Subasta>> ObtenerSubastasGanadasMongoAsync();

        Task<HistorialSubasta> ObtenerHistorialSubastaMongoAsync(Guid idSubasta);



    }
}
