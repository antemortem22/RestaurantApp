using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.Constants;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository.Interface;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ReservaRestaurantContext _restaurantContext;
        private readonly IReservaValidator _validator;

        public ReservaRepository(ReservaRestaurantContext context, IReservaValidator validator)
        {
            _restaurantContext = context;
            _validator = validator;
        }

        public async Task<Respuesta> AddNewReservaAsync(ReservaDTO reserva)
        {
            var fechaHoy = DateTime.Today;

            var newReserva = new Reserva
            {
                CodReserva = Guid.NewGuid().ToString(),
                NombrePersona = reserva.NombrePersona,
                ApellidoPersona = reserva.ApellidoPersona,
                Celular = reserva.Celular,
                Mail = reserva.Mail,
                Dni = reserva.Dni,
                IdRangoReserva = reserva.IdRangoReserva,
                CantidadPersonas = reserva.CantidadPersonas,
                FechaReserva = reserva.FechaReserva.Date,
                FechaAlta = fechaHoy,
                FechaModificacion = fechaHoy,
                Estado = string.Empty
            };

            var validacionReserva = await _validator.ValidacionReservaAsync(newReserva, _restaurantContext);
            if (!validacionReserva.Estado)
            {
                return validacionReserva;
            }

            newReserva.Estado = ReservaEstado.Confirmado;
            await _restaurantContext.Reservas.AddAsync(newReserva);

            var rows = await _restaurantContext.SaveChangesAsync();
            if (rows > 0)
            {
                return validacionReserva;
            }

            validacionReserva.Estado = false;
            validacionReserva.Mensaje.Clear().Append("Error en la api no se agrego la reserva.");
            return validacionReserva;
        }

        public async Task<Respuesta> ModificarNewReservaAsync(ModificacionDTO modificacion)
        {
            var reservaModificar = await _restaurantContext.Reservas
                .FirstOrDefaultAsync(r => r.Dni == modificacion.Dni
                    && r.FechaReserva.Date == modificacion.FechaReserva.Date
                    && r.IdRangoReserva == modificacion.IdRangoReserva);

            if (reservaModificar == null)
            {
                var respuesta = new Respuesta
                {
                    Estado = false
                };
                respuesta.Mensaje.Clear();
                respuesta.Mensaje.Append("No se encontro reserva");
                return respuesta;
            }

            reservaModificar.FechaReserva = (modificacion.FechaModificacion ?? modificacion.FechaReserva).Date;
            reservaModificar.IdRangoReserva = modificacion.IdRangoModificacion;
            reservaModificar.CantidadPersonas = modificacion.CantidadPersonasModificacion;
            reservaModificar.FechaModificacion = DateTime.Today;

            var validacionModificacion = await _validator.ModificacionReservaAsync(reservaModificar, _restaurantContext);
            if (!validacionModificacion.Estado)
            {
                return validacionModificacion;
            }

            await _restaurantContext.SaveChangesAsync();
            return validacionModificacion;
        }

        public async Task<Respuesta> CancelarNewReservaAsync(CancelarDTO cancelar)
        {
            var reservaCancelar = await _restaurantContext.Reservas
                .FirstOrDefaultAsync(r => r.Dni == cancelar.Dni
                    && r.FechaReserva.Date == cancelar.FechaReserva.Date
                    && r.IdRangoReserva == cancelar.IdRangoReserva
                    && r.Estado == ReservaEstado.Confirmado);

            var respuesta = new Respuesta();

            if (reservaCancelar != null)
            {
                reservaCancelar.Estado = ReservaEstado.Cancelado;
                await _restaurantContext.SaveChangesAsync();

                respuesta.Estado = true;
                respuesta.Mensaje.Clear();
                respuesta.Mensaje.Append("Se canceló la reserva.");
                return respuesta;
            }

            respuesta.Estado = false;
            respuesta.Mensaje.Append("No se encontró la reserva.");
            return respuesta;
        }
    }
}
