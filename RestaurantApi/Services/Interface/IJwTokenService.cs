namespace RestaurantApi.Services.Interface
{
    public interface IJwTokenService
    {
        (string Token, DateTimeOffset ExpiresAt) GenerateToken(string username, string role);
    }
}
