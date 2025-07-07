using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre pujas, en el Microservicio Pujas.
    /// </summary>
    public interface IPujaService
    {
        /// <summary>
        /// Método que se encarga de obtener la puja ganadora de una subtasta por su ID en el Microservicio Pujas.
        /// </summary>
        /// <param name="idSubasta">Parametro que corresponde al ID de la subasta a consultar</param>
        /// <returns>Retorna un objeto Puja con su detalle. Si no lo consigue, retorna null</returns>
        Task<Puja> ObtenerPujaGanadoraPorIdSubasta(Guid idSubasta);
        /// <summary>
        /// Método que se encarga de obtener las pujas de una subtasta por su ID en el Microservicio Pujas.
        /// </summary>
        /// <param name="idSubasta">Parametro que corresponde al ID de la subasta a consultar</param>
        /// <returns>Retorna una lista de objetos Puja con su detalle. Si no lo consigue, retorna null</returns>
        Task<List<Puja>> ObtenerPujasSubasta(Guid idSubasta);
    }
}
