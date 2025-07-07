using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Querys
{
    /// <summary>
    /// Clase Query que se encarga de enviar la solicitud para consultar todas las subastas ganadas.
    /// </summary>
    public class ConsultarSubastasGanadasQuery : IRequest<List<ConsultarSubastasGanadasDTO>>
    {
    }
}
