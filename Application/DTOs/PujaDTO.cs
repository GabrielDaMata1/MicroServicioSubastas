using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Objects;

namespace Application.DTOs
{
    public class PujaDTO
    {
        public Guid id { get; set; }
        public Guid idUsuario { get; set; }
        public Guid idSubasta { get; set; }
        public decimal montoPuja { get; set; }
        public decimal montoMaximo { get; set; }
        public string tipoPuja { get; set; }
        public decimal montoPredeterminado { get; set; }
    }
}
