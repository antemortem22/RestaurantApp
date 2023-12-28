using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Services;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarioSemanalController : ControllerBase
    {
        private ICalendarioSemanalService _calendarioService;
        public CalendarioSemanalController(ICalendarioSemanalService calendarioService)
        {
           _calendarioService = calendarioService;
        }

        [HttpGet("Semana")]
        public async Task<IActionResult> Semana()
        {
            var result = await _calendarioService.GetSemanaAsync();
            return Ok(result);
        }

        [HttpGet("Cancelados")]
        public async Task<IActionResult> Cancelados()
        {
            var result = await _calendarioService.GetCanceladosAsync();
            return Ok(result);
        }

        [HttpGet("Confirmados")]
        public async Task<IActionResult> Confirmados()
        {
            var result = await _calendarioService.GetConfirmadosAsync();
            return Ok(result);
        }

        //[HttpGet("SinCupo")]
        //public async Task<IActionResult> SinCupo()
        //{
        //    //Este seguro te tira error
        //    //Me falta agregar para que guarde reservas sin cupos
        //    var result = await _calendarioService.GetSinCupoAsync();
        //    return Ok(result);
        //}

        [HttpGet("DisponibleFecha")]
        public async Task<IActionResult> DisponibleFecha()
        {
            var result = await _calendarioService.GetDisponiblesPorFechaAsync();
            return Ok(result);
        }

    }
}
