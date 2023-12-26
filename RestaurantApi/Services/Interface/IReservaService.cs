using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Services.Interface
{
    public interface IReservaService
    {
        public Task<bool> AddReservaAsync(ReservaDTO reserva);

        public Task<List<Reserva>> GetConfirmadasAsync();

        public Task<List<Reserva>> GetCanceladasAsync();
    }
}
