namespace RestaurantApi.Domain.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
