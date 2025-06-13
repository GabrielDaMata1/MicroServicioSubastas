using Application.Command;
using Application.DTOs;
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
    }
}
