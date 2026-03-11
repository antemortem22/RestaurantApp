using System.Net.Http.Json;
using RestaurantBlazor.Models;

namespace RestaurantBlazor.Services;

public class AuthApiService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly AuthSession _session;

    public AuthApiService(IHttpClientFactory httpFactory, AuthSession session)
    {
        _httpFactory = httpFactory;
        _session = session;
    }

    public async Task<(bool Ok, string Message)> LoginAsync(string username, string password)
    {
        var client = _httpFactory.CreateClient("Api");

        var response = await client.PostAsJsonAsync("api/Auth/login", new LoginRequestDto
        {
            Username = username,
            Password = password
        });

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>();
            return (false, problem?.Detail ?? $"Login fallido ({(int)response.StatusCode}).");
        }

        var payload = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (payload is null || string.IsNullOrWhiteSpace(payload.Token))
        {
            return (false, "Respuesta de login invalida.");
        }

        _session.Set(payload.Token, payload.Role, payload.ExpiresAt);
        return (true, "Login correcto.");
    }

    public void Logout() => _session.Clear();
}
