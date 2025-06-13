using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class ImagenURLProductoVO
    {
        public string url { get; set; }
        public ImagenURLProductoVO(string url)
        {
            this.url = url;
        }
    }
}
