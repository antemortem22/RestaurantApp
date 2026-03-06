using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.Constants;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository.Interface;

namespace RestaurantApi.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ReservaRestaurantContext _restaurantContext;

        public ReservaRepository(ReservaRestaurantContext context)
        {
            _restaurantContext = context;
        }

        public async Task AddAsync(Reserva reserva)
        {
            await _restaurantContext.Reservas.AddAsync(reserva);
        }

        public async Task<Reserva?> GetByCriteriaAsync(string dni, DateTime fechaReserva, int idRangoReserva)
        {
            return await _restaurantContext.Reservas
                .FirstOrDefaultAsync(r => r.Dni == dni
                    && r.FechaReserva.Date == fechaReserva.Date
                    && r.IdRangoReserva == idRangoReserva);
        }

        public async Task<Reserva?> GetConfirmedByCriteriaAsync(string dni, DateTime fechaReserva, int idRangoReserva)
        {
            return await _restaurantContext.Reservas
                .FirstOrDefaultAsync(r => r.Dni == dni
                    && r.FechaReserva.Date == fechaReserva.Date
                    && r.IdRangoReserva == idRangoReserva
                    && r.Estado == ReservaEstado.Confirmado);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _restaurantContext.SaveChangesAsync();
        }
    }
}
