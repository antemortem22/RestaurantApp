using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RestaurantApi.Domain.Common;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("manage-write")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [Authorize(Policy = "CanManageReservas")]
        [HttpPost("RealizarReserva")]
        [ProducesResponseType(typeof(ApiMessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> AddReserva([FromBody] ReservaDTO request)
        {
            var result = await _reservaService.AddReservaAsync(request);
            return ToHttpResult(result);
        }

        [Authorize(Policy = "CanManageReservas")]
        [HttpPut("ModificarReserva")]
        [ProducesResponseType(typeof(ApiMessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ModReserva([FromBody] ModificacionDTO request)
        {
            var result = await _reservaService.ModificarReservaAsync(request);
            return ToHttpResult(result);
        }

        [Authorize(Policy = "CanManageReservas")]
        [HttpDelete("CancelarReserva")]
        [ProducesResponseType(typeof(ApiMessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> CancelarReserva([FromBody] CancelarDTO request)
        {
            var result = await _reservaService.CancelarReservaAsync(request);
            return ToHttpResult(result);
        }

        private IActionResult ToHttpResult(OperationResult result)
        {
            if (result.Success)
            {
                return Ok(new ApiMessageResponse
                {
                    Success = true,
                    Message = result.Message
                });
            }

            return Problem(
                title: "Business validation error",
                detail: result.Message,
                statusCode: StatusCodes.Status400BadRequest,
                type: "https://httpstatuses.com/400");
        }
    }
}
