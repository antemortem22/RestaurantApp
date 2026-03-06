using RestaurantApi.Domain.Common;
using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Services.Interface
{
    public interface IReservaValidator
    {
        Task<OperationResult> ValidateReservaAsync(Reserva reserva);
        Task<OperationResult> ValidateModificacionAsync(Reserva reserva);
    }
}
