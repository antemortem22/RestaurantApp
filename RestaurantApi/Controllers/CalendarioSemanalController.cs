using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    }
}
