using RestaurantApi.Repository.Interface;

namespace RestaurantApi.Services.Interface
{
    public interface ICalendarioSemanalService
    {
        public Task<List<CalendarioInfo>> GetSemanaAsync();
        public Task<List<CalendarioInfo>> GetCanceladosAsync();
        public Task<List<CalendarioInfo>> GetConfirmadosAsync();
        public Task<List<RangoReservaInfo>> GetSinCupoAsync();
        public Task<List<CalendarioInfo>> GetDisponiblesPorFechaAsync();
    }
}
