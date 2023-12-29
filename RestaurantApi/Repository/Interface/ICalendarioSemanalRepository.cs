using RestaurantApi.Domain.Models;

namespace RestaurantApi.Repository.Interface
{
    public interface ICalendarioSemanalRepository
    {
        Task<List<CalendarioResponse>> GetCalendarioSemanalAsync();
        Task<List<CalendarioInfo>> GetCanceladosSemanalAsync();
        Task<List<CalendarioInfo>> GetConfirmadosSemanalAsync();
        Task<List<ListaTurnoInfo>> GetTurnosSinCupoAsync();
        Task<List<ListaTurnoInfo>> GetTurnosDisponiblesPorFechaAsync();
    }



}
