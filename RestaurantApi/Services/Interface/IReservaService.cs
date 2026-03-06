using RestaurantApi.Domain.Common;
using RestaurantApi.Domain.DTO;

namespace RestaurantApi.Services.Interface
{
    public interface IReservaService
    {
        Task<OperationResult> AddReservaAsync(ReservaDTO reserva);
        Task<OperationResult> ModificarReservaAsync(ModificacionDTO modificacion);
        Task<OperationResult> CancelarReservaAsync(CancelarDTO cancelar);
    }
}
