using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository.Interface;
using RestaurantApi.Services.Interface;
using System.Threading;

namespace RestaurantApi.Services
{
    public class ReservaService : IReservaService
    {
        IReservaRepository _repository;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddReservaAsync(ReservaDTO reserva)
        {
            var result = await _repository.AddNewReservaAsync(reserva);

            return result;
        }

        public async Task<List<Reserva>> GetConfirmadasAsync()
        {
            var result = await _repository.GetAllConfirmadasAsync();
            return result;
        }
        public async Task<List<Reserva>> GetCanceladasAsync()
        {
            var result = await _repository.GetAllCanceladasAsync();
            return result;
        }

    }
}
