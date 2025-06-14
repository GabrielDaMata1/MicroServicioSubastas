using Application.Command;
using Application.DTOs;
using Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioSubasta.Controllers
{
    [ApiController]
    [Route("api/Subastas")]
    public class SubastaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubastaController(IMediator mediator)
        {
            _mediator = mediator;

        }
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

        [HttpGet("obtenerSubastas")]
        public async Task<IActionResult> ObtenerSubasta()
        {
            var resultado = await _mediator.Send(new ConsultarSubastasQuery());
                return Ok(resultado);
                
        }

        [HttpGet("obtenerSubasta/{idSubasta}")]
        public async Task<IActionResult> ObtenerSubastaPorId([FromRoute] Guid idSubasta)
        {
            var dto = new ConsultarSubastaDTO {idSubasta = idSubasta };
            var resultado = await _mediator.Send(new ConsultarSubastaQuery(dto));
            return Ok(resultado);

        }

        [HttpGet("obtenerSubastasUsuario/{correo}")]
        public async Task<IActionResult> ObtenerSubastaPorUsuario([FromRoute] string correo)
        {
            var dto = new ConsultarSubastasUsuarioDTO() { correoUsuario = correo };
            var resultado = await _mediator.Send(new ConsultarSubastasUsuarioQuery(dto));
            return Ok(resultado);

        }
    }
}
