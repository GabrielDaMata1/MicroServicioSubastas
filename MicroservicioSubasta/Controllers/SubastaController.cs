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
        public async Task<IActionResult> RegistrarProducto([FromBody] RegistrarSubastaDTO subastaDTO)
        {
            var resultado = await _mediator.Send(new RegistrarSubastaCommand(subastaDTO));
            return Ok(resultado);
        }

    }
}
