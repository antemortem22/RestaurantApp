using System.Net.Http.Json;
using RestaurantBlazor.Models;

namespace RestaurantBlazor.Services;

public class CalendarioApiService
{
    private readonly IHttpClientFactory _httpFactory;

    public CalendarioApiService(IHttpClientFactory httpFactory)
    {
        _httpFactory = httpFactory;
    }

    public async Task<(bool Ok, List<CalendarioResponseDto> Data, string Message)> GetSemanaAsync()
    {
        var client = _httpFactory.CreateClient("Api");
        var response = await client.GetAsync("api/CalendarioSemanal/Calendario");

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return (true, new List<CalendarioResponseDto>(), "Sin datos.");
        }

        if (!response.IsSuccessStatusCode)
        {
            return (false, new List<CalendarioResponseDto>(), $"Error ({(int)response.StatusCode}).");
        }

        var data = await response.Content.ReadFromJsonAsync<List<CalendarioResponseDto>>() ?? new();
        return (true, data, string.Empty);
    }
}
