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

        public async Task<List<CalendarioInfo>> GetSemanaAsync()
        {
            var result = await _repository.GetCalendarioSemanalAsync();

            return result;
        }
        public async Task<List<CalendarioInfo>> GetCanceladosAsync() 
        {
            var result = await _repository.GetCanceladosSemanalAsync();

            return result;
        }
        public async Task<List<CalendarioInfo>> GetConfirmadosAsync() 
        {
            var result = await _repository.GetConfirmadosSemanalAsync();

            return result;
        }
        public async Task<List<RangoReservaInfo>> GetSinCupoAsync() 
        {
            var result = await _repository.GetTurnosSinCupoAsync();

            return result;
        }
        public async Task<List<CalendarioInfo>> GetDisponiblesPorFechaAsync()
        {
            var result = await _repository.GetTurnosDisponiblesPorFechaAsync();

            return result;
        }

    }
}
