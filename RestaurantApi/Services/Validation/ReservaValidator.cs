using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.Constants;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Services.Validation
{
    public class ReservaValidator : IReservaValidator
    {
        public async Task<Respuesta> ValidacionReservaAsync(
            Reserva reserva,
            ReservaRestaurantContext contextrestaurant)
        {
            var respuestaReserva = new Respuesta();

            if (ValidarCampos(reserva, respuestaReserva) &&
                ValidarFecha(reserva, respuestaReserva, reserva.FechaAlta) &&
                await ValidarCantCapacidad(reserva, respuestaReserva, contextrestaurant) &&
                await ValidCantReserva(reserva, respuestaReserva, contextrestaurant))
            {
                respuestaReserva.Mensaje.Clear();
                respuestaReserva.Mensaje.Append("Se generó la reserva con exito.");
                return respuestaReserva;
            }

            return respuestaReserva;
        }

        public async Task<Respuesta> ModificacionReservaAsync(
            Reserva reserva,
            ReservaRestaurantContext contextrestaurant)
        {
            var respuestaReserva = new Respuesta();

            if (ValidarFecha(reserva, respuestaReserva, reserva.FechaModificacion) &&
                await ValidarCantCapacidad(reserva, respuestaReserva, contextrestaurant) &&
                await ValidCantReserva(reserva, respuestaReserva, contextrestaurant))
            {
                respuestaReserva.Mensaje.Clear();
                respuestaReserva.Mensaje.Append("Se modifió la reserva con exito.");
                return respuestaReserva;
            }

            return respuestaReserva;
        }

        private bool ValidarCampos(Reserva reserva, Respuesta respuesta)
        {
            if (ValidarCantidadCaracteres(reserva.Dni, 8, nameof(reserva.Dni), respuesta) ||
                ValidarCantidadCaracteres(reserva.Celular, 20, nameof(reserva.Celular), respuesta) ||
                ValidarCampo(reserva.NombrePersona, respuesta, nameof(reserva.NombrePersona)) ||
                ValidarCampo(reserva.ApellidoPersona, respuesta, nameof(reserva.ApellidoPersona)) ||
                ValidarCampo(reserva.Dni, respuesta, nameof(reserva.Dni)) ||
                ValidarCampo(reserva.Mail, respuesta, nameof(reserva.Mail)) ||
                ValidarCampo(reserva.Celular, respuesta, nameof(reserva.Celular)))
            {
                return respuesta.Estado = false;
            }

            return respuesta.Estado = true;
        }

        private bool ValidarCantidadCaracteres(string valor, int cantidad, string campo, Respuesta respuesta)
        {
            if (valor.Length > cantidad)
            {
                respuesta.Mensaje.Append($"\n{campo} supera los caracteres permitidos: {cantidad}.");
                return true;
            }

            return false;
        }

        private bool ValidarCampo(string valor, Respuesta respuesta, string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                respuesta.Mensaje.Append($"\n{campo}, no tiene datos.");
                return true;
            }

            return false;
        }

        private bool ValidarFecha(Reserva reserva, Respuesta respuesta, DateTime? fechaAlta)
        {
            var fechaActual = fechaAlta;
            var fechaReserva = reserva.FechaReserva;

            if (fechaActual < fechaReserva)
            {
                if (fechaActual >= fechaReserva.AddDays(-7))
                {
                    return respuesta.Estado = true;
                }

                respuesta.Mensaje.Append("\nfecha con mas de 7 dias.");
                return respuesta.Estado = false;
            }

            respuesta.Mensaje.Append("\nfecha antigua, no se puede reservar.");
            return respuesta.Estado = false;
        }

        private async Task<bool> ValidarCantCapacidad(
            Reserva reserva,
            Respuesta respuesta,
            ReservaRestaurantContext reservacontext)
        {
            var capacidadRango = await reservacontext.RangoReservas
                .Where(r => r.IdRangoReserva == reserva.IdRangoReserva)
                .Select(r => r.Cupo)
                .FirstOrDefaultAsync();

            if (reserva.CantidadPersonas <= capacidadRango)
            {
                var reservasEnFecha = await reservacontext.Reservas
                    .Where(r => r.IdRangoReserva == reserva.IdRangoReserva
                        && r.FechaReserva.Date == reserva.FechaReserva.Date
                        && r.Estado == ReservaEstado.Confirmado)
                    .SumAsync(r => r.CantidadPersonas);

                var validacionReserva = capacidadRango - (reservasEnFecha + reserva.CantidadPersonas);

                if (validacionReserva >= 0)
                {
                    return respuesta.Estado = true;
                }

                respuesta.Mensaje.Append($"\nno hay cantidad disponible en el rango, capacidad disponible: {capacidadRango - reservasEnFecha}.");
                return respuesta.Estado = false;
            }

            respuesta.Mensaje.Append($"\nla cantidad excede {capacidadRango}.");
            return respuesta.Estado = false;
        }

        private async Task<bool> ValidCantReserva(
            Reserva reserva,
            Respuesta respuesta,
            ReservaRestaurantContext reservacontext)
        {
            var cantidadDeReservas = await reservacontext.Reservas
                .Where(p => p.NombrePersona == reserva.NombrePersona
                    && p.ApellidoPersona == reserva.ApellidoPersona
                    && p.FechaReserva == reserva.FechaReserva)
                .CountAsync();

            if (cantidadDeReservas == 0)
            {
                return respuesta.Estado = true;
            }

            respuesta.Mensaje.Append("\nel cliente ya tiene una reserva esa fecha.");
            return respuesta.Estado = false;
        }
    }
}
