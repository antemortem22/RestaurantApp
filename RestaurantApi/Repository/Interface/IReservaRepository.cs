using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;


namespace RestaurantApi.Repository.Interface
{
    public interface IReservaRepository
    {
        public Task<bool> AddNewReservaAsync(ReservaDTO reserva);
    }
}
