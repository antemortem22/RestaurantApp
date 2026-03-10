using Microsoft.IdentityModel.Tokens;
using RestaurantApi.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantApi.Services.Security
{
    public class JwTokenService : IJwTokenService
    {
        private readonly IConfiguration _configuration;

        public JwTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTimeOffset ExpiresAt) GenerateToken(string username, string role)
        {
            var key = _configuration["Jwt:Key"]!;
            var issuer = _configuration["Jwt:Issuer"]!;
            var audience = _configuration["Jwt:Audience"]!;
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "120");

            var expiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(expireMinutes);
            var expiresAtArgentina = TimeZoneInfo.ConvertTime(expiresAtUtc, GetArgentinaTimeZone());

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAtUtc.UtcDateTime,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtArgentina);
        }

        private static TimeZoneInfo GetArgentinaTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
            }
            catch
            {
                return TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");
            }
        }
    }
}
