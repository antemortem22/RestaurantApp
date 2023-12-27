using RestaurantApi.Repository.Interface;

namespace RestaurantApi.Services.Interface
{
    public interface ICalendarioSemanalService
    {
        public Task<List<CalendarioInfo>> GetSemana();
    }
}
