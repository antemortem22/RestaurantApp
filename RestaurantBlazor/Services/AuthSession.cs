namespace RestaurantBlazor.Services;

public class AuthSession
{
    public string? Token { get; private set; }
    public string? Role { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }

    public bool IsAuthenticated =>
        !string.IsNullOrWhiteSpace(Token) &&
        ExpiresAt.HasValue &&
        ExpiresAt > DateTimeOffset.Now;

    public void Set(string token, string role, DateTimeOffset expiresAt)
    {
        Token = token;
        Role = role;
        ExpiresAt = expiresAt;
    }

    public void Clear()
    {
        Token = null;
        Role = null;
        ExpiresAt = null;
    }
}