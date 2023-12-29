using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using System.Text;

namespace RestaurantApi.Repository
{
    public class ValidacionClasePrueba
    {
        public async Task<Respuesta> ValidacionReservaAsync(Reserva reserva, 
            ReservaRestaurantContext contextrestaurant)
        {
            var RespuestaReserva = new Respuesta();

            //Genera todas las validaciones y modificaciones en la respuesta
            //de la reserva
            if (ValidarCampos(reserva, RespuestaReserva) &&
                ValidarFecha(reserva, RespuestaReserva, reserva.FechaAlta) &&
                await ValidarCantCapacidad(reserva, RespuestaReserva, contextrestaurant) &&
                await ValidCantReserva(reserva, RespuestaReserva, contextrestaurant))
            {
                RespuestaReserva.Mensaje.Clear();
                RespuestaReserva.Mensaje.Append("Se generó la reserva con exito.");
                return RespuestaReserva;
            }
            //No cumplio con alguna de las validaciones
            return RespuestaReserva;
        }

        public async Task<Respuesta> ModificacionReservaAsync(Reserva reserva, 
            ReservaRestaurantContext contextrestaurant)
        {
            var RespuestaReserva = new Respuesta();
            //Genera todas las validaciones y modificaciones en la respuesta
            //de la modificacion de reserva
            if (ValidarFecha(reserva, RespuestaReserva, reserva.FechaModificacion) &&
                await ValidarCantCapacidad(reserva, RespuestaReserva, contextrestaurant) &&
                await ValidCantReserva(reserva, RespuestaReserva, contextrestaurant))
            {
                RespuestaReserva.Mensaje.Clear();
                RespuestaReserva.Mensaje.Append("Se modifió la reserva con exito.");
                return RespuestaReserva;
            }
            //No cumplio con alguna de las validaciones a modificar
            return RespuestaReserva;

        }
        private bool ValidarCampos(Reserva reserva, Respuesta respuesta)
        {
            if(ValidarCantidadCaracteres(reserva.Dni, 8, nameof(reserva.Dni), respuesta) ||
               ValidarCantidadCaracteres(reserva.Celular, 20, nameof(reserva.Celular), respuesta) ||
               ValidarCampo(reserva.NombrePersona, respuesta, nameof(reserva.NombrePersona)) ||
               ValidarCampo(reserva.ApellidoPersona, respuesta, nameof(reserva.ApellidoPersona)) ||
               ValidarCampo(reserva.Dni, respuesta, nameof(reserva.Dni)) ||
               ValidarCampo(reserva.Mail, respuesta, nameof(reserva.Mail)) ||
               ValidarCampo(reserva.Celular, respuesta, nameof(reserva.Celular)))
            {
                //Uno de los valores es null, "" o sus caracteres superan el limite
                return respuesta.Estado = false;
            }
            //Todos los valores tienen datos en sus campo
            return respuesta.Estado = true;
        }

        private bool ValidarCantidadCaracteres(string reserva, int cantidad, string campo, Respuesta respuesta)
        {
            //Si el valor ingresado supera la cantidad permitida
            if(reserva.Length > cantidad)
            {
                respuesta.Mensaje.Append($"\n{campo} supera los caracteres permitidos: {cantidad}.");
                return true;
            }
            //El campo tiene lo caracteres permitidos
            return false;
        }

        private bool ValidarCampo (string reserva, Respuesta respuesta, string campo)
        {
            if (string.IsNullOrWhiteSpace(reserva))
            {
                //El campo es null o ""
                respuesta.Mensaje.Append($"\n{campo}, no tiene datos.");
                return true;
            }
            //El campo no es nulo
            return false;
        }

        private bool ValidarFecha(Reserva reserva, Respuesta respuesta, DateTime? FechaAlta)
        {
            var FechaActual = FechaAlta;
            var FechaReserva = reserva.FechaReserva;

            //La fecha de reserva no puede ser
            //para dias anteriores a la actual
            if (FechaActual < FechaReserva)
            {
                //Devuelve true si la fecha es valida
                //dentro de 7 dias
                if (FechaActual >= FechaReserva.AddDays(-7)) return respuesta.Estado = true;

                //La fecha tiene mas de 7 dias
                respuesta.Mensaje.Append($"\nfecha con mas de 7 dias.");
                return respuesta.Estado = false;
            }
            //La fecha es de dias pasados
            respuesta.Mensaje.Append($"\nfecha antigua, no se puede reservar.");
            return respuesta.Estado = false;
        }

        private async Task<bool> ValidarCantCapacidad(Reserva reserva, Respuesta respuesta, ReservaRestaurantContext reservacontext)
        {
            var CapacidadRango = await reservacontext.RangoReservas.
                Where(r => r.IdRangoReserva == reserva.IdRangoReserva).
                Select(r => r.Cupo).FirstOrDefaultAsync();

            //Contempla que la cantidad no pase la capacidad del rango
            if(reserva.CantidadPersonas <= CapacidadRango)
            {
                var ReservasEnFecha = await reservacontext.Reservas
                        .Where(r => r.IdRangoReserva == reserva.IdRangoReserva 
                               && r.FechaReserva.Date == reserva.FechaReserva.Date
                               && r.Estado == "CONFIRMADO")
                        .SumAsync(r => r.CantidadPersonas);

                var ValidacionReserva = CapacidadRango - (ReservasEnFecha + reserva.CantidadPersonas);

                //Contempla que la cantidad no se pase de la
                //capacidad de rango si esta tiene reservas echas
                if(ValidacionReserva >= 0)
                {
                    return respuesta.Estado = true;
                }
                //No hay cantidad disponible en el rango
                respuesta.Mensaje.Append($"\nno hay cantidad disponible en el rango, " +
                    $"capacidad disponible: {CapacidadRango - ReservasEnFecha}.");
                return respuesta.Estado = false;
            }
            //La cantidad excede la capacidad
            //del rango
            respuesta.Mensaje.Append($"\nla cantidad excede {CapacidadRango}.");
            return respuesta.Estado = false;
        }

        private async Task<bool> ValidCantReserva(Reserva reserva, Respuesta respuesta, ReservaRestaurantContext reservacontext)
        {
            var CantidadDeReservas = await reservacontext.Reservas
                        .Where(p => p.NombrePersona == reserva.NombrePersona
                        && p.ApellidoPersona == reserva.ApellidoPersona
                        && p.FechaReserva == reserva.FechaReserva).CountAsync();
            //Se fija que la persona no tenga mas de una reserva
            //en la fecha que selecciona
            if(CantidadDeReservas == 0)
            {
                //El cliente no tiene mas
                //de una reserva ese dia
                return respuesta.Estado = true;
            }
            //El cliente ya tenia una
            //reserva ese dia
            respuesta.Mensaje.Append($"\nel cliente ya tiene una reserva esa fecha.");
            return respuesta.Estado = false;
        }

    }

    public class Respuesta
    {
        public bool Estado { get; set; } = true;
        public StringBuilder Mensaje = new StringBuilder("Error en el campo: ");
    }
}
