using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using System.Threading;

namespace RestaurantApi.Repository.Interface
{
    public interface IReservaRepository
    {
        public Task<bool> AddNewReservaAsync(ReservaDTO reserva);

        public  Task<List<Reserva>> GetAllConfirmadasAsync();

        public Task<List<Reserva>> GetAllCanceladasAsync();




    }
}
