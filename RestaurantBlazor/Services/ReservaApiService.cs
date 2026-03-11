using System.Net.Http.Headers;
using System.Net.Http.Json;
using RestaurantBlazor.Models;

namespace RestaurantBlazor.Services;

public class ReservaApiService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly AuthSession _session;

    public ReservaApiService(IHttpClientFactory httpFactory, AuthSession session)
    {
        _httpFactory = httpFactory;
        _session = session;
    }

    private HttpClient CreateAuthClient()
    {
        var client = _httpFactory.CreateClient("Api");
        if (!string.IsNullOrWhiteSpace(_session.Token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _session.Token);
        }
        return client;
    }

    public async Task<(bool Ok, string Message)> CrearAsync(ReservaRequestDto dto)
    {
        var response = await CreateAuthClient().PostAsJsonAsync("api/Reserva/RealizarReserva", dto);
        return await ParseResponse(response);
    }

    public async Task<(bool Ok, string Message)> ModificarAsync(ModificacionRequestDto dto)
    {
        var response = await CreateAuthClient().PutAsJsonAsync("api/Reserva/ModificarReserva", dto);
        return await ParseResponse(response);
    }

    public async Task<(bool Ok, string Message)> CancelarAsync(CancelarRequestDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "api/Reserva/CancelarReserva")
        {
            Content = JsonContent.Create(dto)
        };

        var response = await CreateAuthClient().SendAsync(request);
        return await ParseResponse(response);
    }

    private static async Task<(bool Ok, string Message)> ParseResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var ok = await response.Content.ReadFromJsonAsync<ApiMessageResponseDto>();
            return (true, ok?.Message ?? "Operacion exitosa.");
        }

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>();
        return (false, problem?.Detail ?? $"Error ({(int)response.StatusCode}).");
    }
}
