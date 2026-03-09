using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("public-read")]
    public class CalendarioSemanalController : ControllerBase
    {
        private readonly ICalendarioSemanalService _calendarioService;

        public CalendarioSemanalController(ICalendarioSemanalService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        [HttpGet("Calendario")]
        public async Task<IActionResult> Semana()
        {
            var result = await _calendarioService.GetSemanaAsync();
            return ToCollectionResult(result);
        }

        [HttpGet("Cancelados")]
        public async Task<IActionResult> Cancelados()
        {
            var result = await _calendarioService.GetCanceladosAsync();
            return ToCollectionResult(result);
        }

        [HttpGet("Confirmados")]
        public async Task<IActionResult> Confirmados()
        {
            var result = await _calendarioService.GetConfirmadosAsync();
            return ToCollectionResult(result);
        }

        [HttpGet("SinCupo")]
        public async Task<IActionResult> SinCupo()
        {
            var result = await _calendarioService.GetSinCupoAsync();
            return ToCollectionResult(result);
        }

        [HttpGet("DisponibleFecha")]
        public async Task<IActionResult> DisponibleFecha()
        {
            var result = await _calendarioService.GetDisponiblesPorFechaAsync();
            return ToCollectionResult(result);
        }

        private IActionResult ToCollectionResult<T>(List<T>? result)
        {
            if (result is null)
            {
                return Problem(
                    title: "Data retrieval error",
                    detail: "No se pudo recuperar la informacion solicitada.",
                    statusCode: StatusCodes.Status500InternalServerError,
                    type: "https://httpstatuses.com/500");
            }

            if (result.Count == 0)
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}
