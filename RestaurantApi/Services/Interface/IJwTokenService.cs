namespace RestaurantApi.Services.Interface
{
    public interface IJwTokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(string username, string role);
    }
}
