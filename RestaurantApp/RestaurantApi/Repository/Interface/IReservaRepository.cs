using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;


namespace RestaurantApi.Repository.Interface
{
    public interface IReservaRepository
    {
        public Task<Respuesta> AddNewReservaAsync(ReservaDTO reserva);
        public Task<Respuesta> ModificarNewReservaAsync(ModificacionDTO modificacion);
        public Task<Respuesta> CancelarNewReservaAsync(string id);
    }
}
