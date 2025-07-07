using Application.Command;
using Application.DTOs;
using Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioSubasta.Controllers
{
    /// <summary>
    /// Clase controller API encargada de procesar las solicitudes de inserción, modificación, eliminación y consulta,
    /// sobre las subastas.
    /// </summary>
    [ApiController]
    [Route("api/Subastas")]
    public class SubastaController : ControllerBase
    {
        /// <summary>
        /// Atributo que se encarga de enviar solicitudes (commands/queries) mediante el patrón mediador
        /// </summary>
        private readonly IMediator _mediator;

        public SubastaController(IMediator mediator)
        {
            _mediator = mediator;

        }

        /// <summary>
        /// Endpoint encargado de registrar una nueva subasta.
        /// </summary>
        /// <param name="subastaDTO">Parametro de tipo DTO con los datos de la subasta a registrar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>
        [HttpPost("registroSubasta")]
        public async Task<IActionResult> RegistrarSubasta([FromBody] RegistrarSubastaDTO subastaDTO)
        {
            var resultado = await _mediator.Send(new RegistrarSubastaCommand(subastaDTO));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "La subasta se registró exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO{ Mensaje = "La subasta no pudo ser registrada.", Exito = false });
        }

        /// <summary>
        /// Endpoint encargado de modificar una subasta.
        /// </summary>
        /// <param name="subastaModificarDto">Parametro de tipo DTO con los datos de la subasta a modificar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>
        [HttpPut("modificarSubasta")]
        public async Task<IActionResult> ModificarSubasta([FromBody] ModificarSubastaDTO subastaModificarDto)
        {
            var resultado = await _mediator.Send(new ModificarSubastaCommand(subastaModificarDto));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "La subasta se modificó exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "La subasta no pudo ser modificada.", Exito = false });
        }

        /// <summary>
        /// Endpoint encargado de eliminar una subasta.
        /// </summary>
        /// <param name="subastaEliminarDto">Parametro de tipo DTO con los datos de la subasta a aliminar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>
        [HttpDelete("eliminarSubasta")]
        public async Task<IActionResult> EliminarSubasta([FromBody] EliminarSubastaDTO subastaEliminarDto)
        {
            var resultado = await _mediator.Send(new EliminarSubastaCommand(subastaEliminarDto));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "La subasta se eliminó exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "La subasta no pudo ser eliminada.", Exito = false });
        }

        /// <summary>
        /// Endpoint encargado de consultar las subastas.
        /// </summary>
        /// <returns>Retorna una lista de DTOs con el detalle de cada subasta.</returns>
        [HttpGet("obtenerSubastas")]
        public async Task<IActionResult> ObtenerSubasta()
        {
            var resultado = await _mediator.Send(new ConsultarSubastasQuery());
                return Ok(resultado);
                
        }

        /// <summary>
        /// Endpoint encargado de consultar las subastas.
        /// </summary>
        /// <param name="idSubasta">Parametro que corresponde al ID de la subasta a consultar.</param>
        /// <returns>Retorna una lista de DTOs con el detalle de cada subasta.</returns>

        [HttpGet("obtenerSubasta/{idSubasta}")]
        public async Task<IActionResult> ObtenerSubastaPorId([FromRoute] Guid idSubasta)
        {
            var dto = new ConsultarSubastaDTO {idSubasta = idSubasta };
            var resultado = await _mediator.Send(new ConsultarSubastaQuery(dto));
            return Ok(resultado);

        }

        /// <summary>
        /// Endpoint encargado de consultar las subastas de un subastador.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del subastador de las subastas a consultar.</param>
        /// <returns>Retorna una lista de DTOs con el detalle de cada subasta.</returns>

        [HttpGet("obtenerSubastasUsuario/{correo}")]
        public async Task<IActionResult> ObtenerSubastaPorUsuario([FromRoute] string correo)
        {
            var dto = new ConsultarSubastasUsuarioDTO() { correoUsuario = correo };
            var resultado = await _mediator.Send(new ConsultarSubastasUsuarioQuery(dto));
            return Ok(resultado);

        }

        /// <summary>
        /// Endpoint encargado de consultar las subastas ganadas con sus pujas.
        /// </summary>
        /// <returns>Retorna una lista de DTOs con el detalle de cada subasta y sus pujas.</returns>
        [HttpGet("obtenerSubastasGanadasPujas/")]
        public async Task<IActionResult> obtenerSubastasGanadasPujas()
        {
            var resultado = await _mediator.Send(new ConsultarSubastasGanadasPujasQuery());
            return Ok(resultado);

        }

        /// <summary>
        /// Endpoint encargado de consultar las subastas ganadas por un usuario.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del usuario de las subastas ganadas a consultar.</param>
        /// <returns>Retorna una lista de DTOs con el detalle de cada subasta.</returns>
        [HttpGet("obtenerSubastasGanadasUsuario/{correo}")]
        public async Task<IActionResult> ObtenerSubastaGanadasPorUsuario([FromRoute] string correo)
        {
            var dto = new ConsultarSubastasUsuarioDTO() { correoUsuario = correo };
            var resultado = await _mediator.Send(new ConsultarSubastasGanadasUsuarioQuery(dto));
            return Ok(resultado);

        }

        /// <summary>
        /// Endpoint encargado de consultar las subastas ganadas y su puja ganadora.
        /// </summary>
        /// <returns>Retorna una lista de DTOs con el detalle de cada subasta y su puja ganadora.</returns>

        [HttpGet("obtenerSubastasGanadas")]
        public async Task<IActionResult> ObtenerSubastaGanadas()
        {
            var resultado = await _mediator.Send(new ConsultarSubastasGanadasQuery());
            return Ok(resultado);

        }
    }
}
