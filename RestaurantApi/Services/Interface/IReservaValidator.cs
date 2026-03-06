using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository;

namespace RestaurantApi.Services.Interface
{
    public interface IReservaValidator
    {
        Task<Respuesta> ValidacionReservaAsync(Reserva reserva, ReservaRestaurantContext contextrestaurant);
        Task<Respuesta> ModificacionReservaAsync(Reserva reserva, ReservaRestaurantContext contextrestaurant);
    }
}