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
           
            var newReserva = new Reserva();

            //Generacion de codigo unico para la reserva
            Guid id = Guid.NewGuid();

            newReserva.CodReserva = id.ToString();
            newReserva.NombrePersona = reserva.NombrePersona; 
            newReserva.ApellidoPersona = reserva.ApellidoPersona;
            newReserva.Celular = reserva.Celular;
            newReserva.Mail = reserva.Mail;
            newReserva.Dni = reserva.Dni;
            newReserva.IdRangoReserva = reserva.IdRangoReserva;
            newReserva.CantidadPersonas = reserva.CantidadPersonas;
            newReserva.FechaReserva = reserva.FechaReserva;
            newReserva.FechaAlta = DateTime.Now;
            newReserva.FechaModificacion = DateTime.Now;

            //Aca no se si queres validar la reserva de otra forma
            //Puse un if por ahora para comprobar si funcionaba
            if (ValidacionesReserva(newReserva, _restaurantContext))
            {
                //Restar a los cupos por turno
                var rangoReserva = await _restaurantContext.RangoReservas.FindAsync(reserva.IdRangoReserva);

                if (rangoReserva != null)
                {
                    // Restar la cantidad de personas del cupo
                    rangoReserva.Cupo -= reserva.CantidadPersonas;

                    // Guardar los cambios en la base de datos
                    await _restaurantContext.SaveChangesAsync();
                }


                reserva.Estado.ToUpper();

                await _restaurantContext.Reservas.AddAsync(newReserva);

                int rows = await _restaurantContext.SaveChangesAsync();

                return rows > 0;
            }
            return false;
        }

        public async Task<List<Reserva>> GetAllConfirmadasAsync()
        {
            return await _restaurantContext.Reservas
                                      .Where(r => r.Estado == "CONFIRMADO")
                                      .ToListAsync();
        }

        public async Task<List<Reserva>> GetAllCanceladasAsync()
        {
            return await _restaurantContext.Reservas
                                      .Where(r => r.Estado == "CANCELADO")
                                      .ToListAsync();
        }

        //Validaciones de la reserva, me falta mejorar el codigo un toque mas.
        //Esta maso pero de momento funciona.

        public bool ValidacionesReserva(Reserva reserva, ReservaRestaurantContext contextrestaurant)
        {
            if (ValidCampos(reserva)
                && (ValidFecha(reserva) || ValidCantReserva(reserva, contextrestaurant))
                && (ValidCantCapacidad(reserva, contextrestaurant)))
            {
                return true;
            }
            return false;
        }

        public bool ValidCampos(Reserva reserva)
        {
            if (ValidNull(reserva.NombrePersona) || ValidNull(reserva.ApellidoPersona)
                || ValidNull(reserva.Dni) || ValidNull(reserva.Mail) || ValidNull(reserva.Celular))
            {
                return false;
            }
            return true;
        }

        public bool ValidNull(string valores)
        {
            return string.IsNullOrWhiteSpace(valores);
        }

        public bool ValidFecha(Reserva reserva)
        {
            var FechaActual = DateTime.Today;
            var FechaReserva = reserva.FechaReserva.AddDays(-7);

            if (FechaActual == FechaReserva || FechaActual > FechaReserva) return true;
            return false;
        }

        public bool ValidCantCapacidad(Reserva reserva, ReservaRestaurantContext reservacontext)
        {
            var cantconfirm = 100 - (reservacontext.Reservas
                .Where(p => p.FechaReserva == reserva.FechaReserva && p.Estado.ToLower() == "confirmado")
                .Count());
            return cantconfirm > reserva.CantidadPersonas && reserva.CantidadPersonas > 0;
        }

        public bool ValidCantReserva(Reserva reserva, ReservaRestaurantContext reservacontext)
        {
            var Valor = reservacontext.Reservas
                .Where(p => p.NombrePersona == reserva.NombrePersona
                && p.ApellidoPersona == reserva.ApellidoPersona).Count();
            return Valor < 1;
        }
    }
}
