using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.DTO;
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

        public async Task<bool> AddNewReservaAsync(ReservaDTO reserva)
        {
            // Generación de código único para la reserva
            Guid id = Guid.NewGuid();

            var newReserva = new Reserva
            {
                CodReserva = id.ToString(),
                NombrePersona = reserva.NombrePersona,
                ApellidoPersona = reserva.ApellidoPersona,
                Celular = reserva.Celular,
                Mail = reserva.Mail,
                Dni = reserva.Dni,
                IdRangoReserva = reserva.IdRangoReserva,
                CantidadPersonas = reserva.CantidadPersonas,
                FechaReserva = reserva.FechaReserva,
                FechaAlta = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            // Validar si hay cupo disponible para la fecha y el rango de reserva
            var rangoReserva = await _restaurantContext.RangoReservas
                .Where(rr => rr.IdRangoReserva == reserva.IdRangoReserva)
                .FirstOrDefaultAsync();

            if (rangoReserva != null)
            {
                var reservasEnFecha = await _restaurantContext.Reservas
                    .Where(r => r.IdRangoReserva == reserva.IdRangoReserva && r.FechaReserva.Date == reserva.FechaReserva.Date)
                    .SumAsync(r => r.CantidadPersonas);

                int cupoDisponible = rangoReserva.Cupo - reservasEnFecha - reserva.CantidadPersonas;

                if (cupoDisponible < 0)
                {
                    // No hay suficiente cupo disponible para la reserva en esa fecha y rango
                    return false;
                }
            }
            else
            {
                // El ID de rango de reserva proporcionado no es válido
                return false;
            }

            
            await _restaurantContext.Reservas.AddAsync(newReserva);

            int rows = await _restaurantContext.SaveChangesAsync();

            return rows > 0;
        }


    }
}
