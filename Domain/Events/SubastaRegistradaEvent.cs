using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Events
{
    public record SubastaRegistradaEvent(Subasta subasta,Guid idUsuario);
}
