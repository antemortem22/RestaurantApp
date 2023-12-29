using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository;
using RestaurantApi.Repository.Interface;
using RestaurantApi.Services.Interface;


namespace RestaurantApi.Services
{
    public class ReservaService : IReservaService
    {
        IReservaRepository _repository;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository;
        }

        public async Task<Respuesta> AddReservaAsync(ReservaDTO reserva)
        {
            var result = await _repository.AddNewReservaAsync(reserva);

            return result;
        }

        public async Task<Respuesta> ModificarReservaAsync(ModificacionDTO modificacion)
        {
            var result = await _repository.ModificarNewReservaAsync(modificacion);

            return result;
        }

        public async Task<Respuesta> CancelarReservaAsync(CancelarDTO cancelar)
        {
            var result = await _repository.CancelarNewReservaAsync(cancelar);

            return result;
        }
    }
}
