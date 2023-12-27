using RestaurantApi.Repository.Interface;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Services
{
    public class CalendarioSemanalService : ICalendarioSemanalService
    {
        ICalendarioSemanalRepository _repository;

        public CalendarioSemanalService(ICalendarioSemanalRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CalendarioInfo>> GetSemana()
        {
            var result = await _repository.GetCalendarioSemanal();

            return result;
        }
    }
}
