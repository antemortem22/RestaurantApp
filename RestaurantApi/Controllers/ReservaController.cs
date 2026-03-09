using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Domain.Common;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [Authorize(Policy = "CanManageReservas")]
        [HttpPost("RealizarReserva")]
        public async Task<IActionResult> AddReserva([FromBody] ReservaDTO request)
        {
            var result = await _reservaService.AddReservaAsync(request);
            return ToHttpResult(result);
        }

        [Authorize(Policy = "CanManageReservas")]
        [HttpPut("ModificarReserva")]
        public async Task<IActionResult> ModReserva([FromBody] ModificacionDTO request)
        {
            var result = await _reservaService.ModificarReservaAsync(request);
            return ToHttpResult(result);
        }

        [Authorize(Policy = "CanManageReservas")]
        [HttpDelete("CancelarReserva")]
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
