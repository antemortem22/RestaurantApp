using RestaurantApi.Domain.Models;


namespace RestaurantApi.Services.Interface
{
    public interface ICalendarioSemanalService
    {
        public Task<List<CalendarioResponse>> GetSemanaAsync();
        public Task<List<CalendarioInfo>> GetCanceladosAsync();
        public Task<List<CalendarioInfo>> GetConfirmadosAsync();
        public Task<List<ListaTurnoInfo>> GetSinCupoAsync();
        public Task<List<ListaTurnoInfo>> GetDisponiblesPorFechaAsync();
    }
}
