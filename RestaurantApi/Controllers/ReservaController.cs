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
        //Controller improvisado para probar si funciona AddReserva
        //Funciona

        private IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReserva([FromBody] ReservaDTO request)
        {
            var result = await _reservaService.AddReservaAsync(request);

            if (result)
            {
                return Ok("Se agrego reserva");
            }
            return BadRequest("No se agrego reserva");
        }
    }
}
