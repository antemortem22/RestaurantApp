using Microsoft.Extensions.Caching.Memory;
using RestaurantApi.Domain.Models;
using RestaurantApi.Repository.Interface;
using RestaurantApi.Services.Cache;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Services
{
    public class CalendarioSemanalService : ICalendarioSemanalService
    {
        private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(45);

        private readonly ICalendarioSemanalRepository _repository;
        private readonly IMemoryCache _cache;

        public CalendarioSemanalService(ICalendarioSemanalRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public Task<List<CalendarioResponse>> GetSemanaAsync() =>
            GetOrCreateAsync(CalendarioCacheKeys.Semana, () => _repository.GetCalendarioSemanalAsync());

        public Task<List<CalendarioInfo>> GetCanceladosAsync() =>
            GetOrCreateAsync(CalendarioCacheKeys.Cancelados, () => _repository.GetCanceladosSemanalAsync());

        public Task<List<CalendarioInfo>> GetConfirmadosAsync() =>
            GetOrCreateAsync(CalendarioCacheKeys.Confirmados, () => _repository.GetConfirmadosSemanalAsync());

        public Task<List<ListaTurnoInfo>> GetSinCupoAsync() =>
            GetOrCreateAsync(CalendarioCacheKeys.SinCupo, () => _repository.GetTurnosSinCupoAsync());

        public Task<List<ListaTurnoInfo>> GetDisponiblesPorFechaAsync() =>
            GetOrCreateAsync(CalendarioCacheKeys.DisponibleFecha, () => _repository.GetTurnosDisponiblesPorFechaAsync());

        private async Task<List<T>> GetOrCreateAsync<T>(string key, Func<Task<List<T>>> factory)
        {
            if (_cache.TryGetValue(key, out List<T>? cached) && cached is not null)
            {
                return cached;
            }

            var data = await factory();

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheTtl
            };

            _cache.Set(key, data, options);
            return data;
        }
    }
}
