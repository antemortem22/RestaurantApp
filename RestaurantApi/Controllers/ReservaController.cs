using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost("RealizarReserva")]
        public async Task<IActionResult> AddReserva([FromBody] ReservaDTO request)
        {
            var result = await _reservaService.AddReservaAsync(request);
            var mensaje = result.Mensaje.ToString();
            if (result.Estado) return Ok(mensaje);
            return BadRequest(mensaje);
        }

        [HttpPost("ModificarReserva")]
        public async Task<IActionResult> ModReserva([FromBody] ModificacionDTO request)
        {
            var result = await _reservaService.ModificarReservaAsync(request);
            var mensaje = result.Mensaje.ToString();
            if (result.Estado) return Ok(mensaje);
            return BadRequest(mensaje);
        }

        [HttpPost("CancelarReserva")]
        public async Task<IActionResult> CancelarReserva([FromQuery] string id)
        {
            var result = await _reservaService.CancelarReservaAsync(id);
            var mensaje = result.Mensaje.ToString();
            if (result.Estado) return Ok(mensaje);
            return BadRequest(mensaje);
        }
    }
}
