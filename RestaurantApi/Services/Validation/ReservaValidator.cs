using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.Common;
using RestaurantApi.Domain.Constants;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository;
using RestaurantApi.Services.Interface;
using System.Text;

namespace RestaurantApi.Services.Validation
{
    public class ReservaValidator : IReservaValidator
    {
        private readonly ReservaRestaurantContext _context;

        public ReservaValidator(ReservaRestaurantContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> ValidateReservaAsync(Reserva reserva)
        {
            var message = new StringBuilder("Error en el campo: ");

            if (ValidarCampos(reserva, message) &&
                ValidarFecha(reserva, message, reserva.FechaAlta) &&
                await ValidarCantCapacidad(reserva, message) &&
                await ValidCantReserva(reserva, message))
            {
                return OperationResult.Ok("Se genero la reserva con exito.");
            }

            return OperationResult.Fail(message.ToString());
        }

        public async Task<OperationResult> ValidateModificacionAsync(Reserva reserva)
        {
            var message = new StringBuilder("Error en el campo: ");

            if (ValidarFecha(reserva, message, reserva.FechaModificacion) &&
                await ValidarCantCapacidad(reserva, message) &&
                await ValidCantReserva(reserva, message))
            {
                return OperationResult.Ok("Se modifico la reserva con exito.");
            }

            return OperationResult.Fail(message.ToString());
        }

        private static bool ValidarCampos(Reserva reserva, StringBuilder message)
        {
            if (ValidarCantidadCaracteres(reserva.Dni, 8, nameof(reserva.Dni), message) ||
                ValidarCantidadCaracteres(reserva.Celular, 20, nameof(reserva.Celular), message) ||
                ValidarCampo(reserva.NombrePersona, message, nameof(reserva.NombrePersona)) ||
                ValidarCampo(reserva.ApellidoPersona, message, nameof(reserva.ApellidoPersona)) ||
                ValidarCampo(reserva.Dni, message, nameof(reserva.Dni)) ||
                ValidarCampo(reserva.Mail, message, nameof(reserva.Mail)) ||
                ValidarCampo(reserva.Celular, message, nameof(reserva.Celular)))
            {
                return false;
            }

            return true;
        }

        private static bool ValidarCantidadCaracteres(string valor, int cantidad, string campo, StringBuilder message)
        {
            if (valor.Length > cantidad)
            {
                message.Append($"\n{campo} supera los caracteres permitidos: {cantidad}.");
                return true;
            }

            return false;
        }

        private static bool ValidarCampo(string valor, StringBuilder message, string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                message.Append($"\n{campo}, no tiene datos.");
                return true;
            }

            return false;
        }

        private static bool ValidarFecha(Reserva reserva, StringBuilder message, DateTime? fechaBase)
        {
            var fechaActual = fechaBase;
            var fechaReserva = reserva.FechaReserva;

            if (fechaActual < fechaReserva)
            {
                if (fechaActual >= fechaReserva.AddDays(-7))
                {
                    return true;
                }

                message.Append("\nfecha con mas de 7 dias.");
                return false;
            }

            message.Append("\nfecha antigua, no se puede reservar.");
            return false;
        }

        private async Task<bool> ValidarCantCapacidad(Reserva reserva, StringBuilder message)
        {
            var capacidadRango = await _context.RangoReservas
                .Where(r => r.IdRangoReserva == reserva.IdRangoReserva)
                .Select(r => r.Cupo)
                .FirstOrDefaultAsync();

            if (reserva.CantidadPersonas <= capacidadRango)
            {
                var reservasEnFecha = await _context.Reservas
                    .Where(r => r.IdRangoReserva == reserva.IdRangoReserva
                        && r.FechaReserva.Date == reserva.FechaReserva.Date
                        && r.Estado == ReservaEstado.Confirmado)
                    .SumAsync(r => r.CantidadPersonas);

                var validacionReserva = capacidadRango - (reservasEnFecha + reserva.CantidadPersonas);

                if (validacionReserva >= 0)
                {
                    return true;
                }

                message.Append($"\nno hay cantidad disponible en el rango, capacidad disponible: {capacidadRango - reservasEnFecha}.");
                return false;
            }

            message.Append($"\nla cantidad excede {capacidadRango}.");
            return false;
        }

        private async Task<bool> ValidCantReserva(Reserva reserva, StringBuilder message)
        {
            var cantidadDeReservas = await _context.Reservas
                .Where(p => p.NombrePersona == reserva.NombrePersona
                    && p.ApellidoPersona == reserva.ApellidoPersona
                    && p.FechaReserva == reserva.FechaReserva)
                .CountAsync();

            if (cantidadDeReservas == 0)
            {
                return true;
            }

            message.Append("\nel cliente ya tiene una reserva esa fecha.");
            return false;
        }
    }
}
