using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository;

namespace RestaurantApi.Services.Interface
{
    public interface IReservaService
    {
        public Task<Respuesta> AddReservaAsync(ReservaDTO reserva);
        public Task<Respuesta> ModificarReservaAsync(ModificacionDTO modificacion);

        public Task<Respuesta> CancelarReservaAsync(string id);

    }
}
