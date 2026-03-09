using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwTokenService _jwTokenService;

        public AuthController(IJwTokenService jwTokenService)
        {
            _jwTokenService = jwTokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            // Demo challenge: solo usuario mesero para operaciones de escritura.
            if (request.Username == "mesero" && request.Password == "1234")
            {
                var (token, expiresAt) = _jwTokenService.GenerateToken(request.Username, "Mesero");
                return Ok(new LoginResponseDTO { Token = token, ExpiresAt = expiresAt, Role = "Mesero" });
            }

            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = "Credenciales invalidas.",
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }
}
