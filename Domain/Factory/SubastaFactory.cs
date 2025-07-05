using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;

namespace Domain.Factory
{
    public static class SubastaFactory
    {
        public static Subasta CrearSubasta(string nombre, string descripcion, Guid idProducto, DateTime fechaInicio, DateTime fechaFin, decimal incrementoMinimo, decimal precioReserva, string estado)
        {
            var nombreVO = new NombreSubastaVO(nombre);
            var descripcionVO = new DescripcionSubastaVO(descripcion);
            var productoId = idProducto;
            var fechaInicioVO = new FechaInicioSubastaVO(fechaInicio);
            var fechaFinVO = new FechaFinSubastaVO(fechaFin);
            var incrementoMinimoVO = new IncrementoMinimoSubastaVO(incrementoMinimo);
            var precioReservaVO = new PrecioReservaSubastaVO(precioReserva);

            return new Subasta(nombreVO, descripcionVO, productoId,fechaInicioVO,fechaFinVO,incrementoMinimoVO,precioReservaVO);
        }

        public static Subasta CrearSubastaConId(Guid id, string nombre, string descripcion, Guid idProducto, DateTime fechaInicio, DateTime fechaFin, decimal incrementoMinimo, decimal precioReserva, string estado)
        {
            var nombreVO = new NombreSubastaVO(nombre);
            var descripcionVO = new DescripcionSubastaVO(descripcion);
            var productoId = idProducto;
            var fechaInicioVO = new FechaInicioSubastaVO(fechaInicio);
            var fechaFinVO = new FechaFinSubastaVO(fechaFin);
            var incrementoMinimoVO = new IncrementoMinimoSubastaVO(incrementoMinimo);
            var precioReservaVO = new PrecioReservaSubastaVO(precioReserva);
            var estadoVO = new EstadoSubastaVO(estado);
            return new Subasta(id,nombreVO, descripcionVO, productoId, fechaInicioVO, fechaFinVO, incrementoMinimoVO, precioReservaVO, estadoVO);
        }

        public static Subasta CrearSubastaConIdUsuario(Guid id, string nombre, string descripcion, Guid idProducto, DateTime fechaInicio, DateTime fechaFin, decimal incrementoMinimo, decimal precioReserva, string estado, Guid idUsuario)
        {
            var nombreVO = new NombreSubastaVO(nombre);
            var descripcionVO = new DescripcionSubastaVO(descripcion);
            var productoId = idProducto;
            var fechaInicioVO = new FechaInicioSubastaVO(fechaInicio);
            var fechaFinVO = new FechaFinSubastaVO(fechaFin);
            var incrementoMinimoVO = new IncrementoMinimoSubastaVO(incrementoMinimo);
            var precioReservaVO = new PrecioReservaSubastaVO(precioReserva);
            var estadoVO = new EstadoSubastaVO(estado);
            return new Subasta(id, nombreVO, descripcionVO, productoId, fechaInicioVO, fechaFinVO, incrementoMinimoVO, precioReservaVO, estadoVO, idUsuario);
        }

    }
}
