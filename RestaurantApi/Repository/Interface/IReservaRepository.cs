using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Repository.Interface
{
    public interface IReservaRepository
    {
        Task AddAsync(Reserva reserva);
        Task<Reserva?> GetByCriteriaAsync(string dni, DateTime fechaReserva, int idRangoReserva);
        Task<Reserva?> GetConfirmedByCriteriaAsync(string dni, DateTime fechaReserva, int idRangoReserva);
        Task<int> SaveChangesAsync();
    }
}
