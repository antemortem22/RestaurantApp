using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository.Interface;


namespace RestaurantApi.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ReservaRestaurantContext _restaurantContext;

        private ValidacionClasePrueba Validacion;

        public ReservaRepository(ReservaRestaurantContext context)
        {
            _restaurantContext = context;
            Validacion = new ValidacionClasePrueba();
        }

        public async Task<Respuesta> AddNewReservaAsync(ReservaDTO reserva)
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
                FechaModificacion = DateTime.Now,
                Estado = ""
            };

            #region Codigo Agos
            //// Validar si hay cupo disponible para la fecha y el rango de reserva
            //var rangoReserva = await _restaurantContext.RangoReservas
            //    .Where(rr => rr.IdRangoReserva == reserva.IdRangoReserva)
            //    .FirstOrDefaultAsync();

            //if (rangoReserva != null)
            //{
            //    var reservasEnFecha = await _restaurantContext.Reservas
            //        .Where(r => r.IdRangoReserva == reserva.IdRangoReserva && r.FechaReserva.Date == reserva.FechaReserva.Date)
            //        .SumAsync(r => r.CantidadPersonas);

            //    int cupoDisponible = rangoReserva.Cupo - reservasEnFecha - reserva.CantidadPersonas;

            //    if (cupoDisponible < 0)
            //    {
            //        // No hay suficiente cupo disponible para la reserva en esa fecha y rango
            //        return false;
            //    }
            //}
            #endregion

            var ValidacionReserva = await Validacion.ValidacionReservaAsync(newReserva, _restaurantContext);

            if (ValidacionReserva.Estado)
            {
                newReserva.Estado = "CONFIRMADO";
                await _restaurantContext.Reservas.AddAsync(newReserva);

                int rows = await _restaurantContext.SaveChangesAsync();

                if(rows > 0)
                {
                    //La reserva se realizo
                    return ValidacionReserva;
                }
                ValidacionReserva.Estado = false;
                ValidacionReserva.Mensaje.Clear().Append("Error en la api no se agrego la reserva.");
            }
            //La reserva no se realizo por falta de
            //cumplir una validacion
            return ValidacionReserva;
        }

        public async Task<Respuesta> ModificarNewReservaAsync(ModificacionDTO modificacion)
        {
            var ReservaModificar = await _restaurantContext.Reservas.
                FirstOrDefaultAsync(r => r.CodReserva == modificacion.CodReserva);

            ReservaModificar.FechaReserva = modificacion.FechaReserva;
            ReservaModificar.IdRangoReserva = modificacion.IdRangoReserva;
            ReservaModificar.CantidadPersonas = modificacion.CantidadPersonas;
            ReservaModificar.FechaModificacion = DateTime.Now;

            var ValidacionModificacion = await Validacion.
                ModificacionReservaAsync(ReservaModificar, _restaurantContext);

            if (ValidacionModificacion.Estado)
            {

                await _restaurantContext.SaveChangesAsync();

                //La modificacion se realizo
                return ValidacionModificacion;
            }
            //La modificacion no se realizo por falta de
            //cumplir una validacion
            return ValidacionModificacion;
        }

        public async Task<Respuesta> CancelarNewReservaAsync(string id)
        {
            //Se bussca reserva para cancelar
            var ReservaCancelar = await _restaurantContext.Reservas.
                FirstOrDefaultAsync(r => r.CodReserva == id && 
                r.Estado == "CONFIRMADO");

            Respuesta respuesta = new Respuesta();

            if(ReservaCancelar != null)
            {
                ReservaCancelar.Estado = "CANCELADO";

                await _restaurantContext.SaveChangesAsync();

                respuesta.Estado = true;
                respuesta.Mensaje.Clear();
                respuesta.Mensaje.Append("Se cancelo la reserva.");
                return respuesta;
            }
            //No se encontro la reserva en la bdd
            respuesta.Estado = false;
            respuesta.Mensaje.Append("No se encontró la reserva.");
            return respuesta;
        }
    }
}
