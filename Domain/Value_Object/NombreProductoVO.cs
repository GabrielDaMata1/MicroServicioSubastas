using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Value_Object
{
    public class NombreProductoVO
    {
        public string Nombre { get; set; }

        [JsonConstructor]
        public NombreProductoVO(string nombre)
        {
            Nombre = nombre;
        }
    }
}