using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository.Interface;
using System.Text;


namespace RestaurantApi.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ReservaRestaurantContext _restaurantContext;

        private ValidacionClasePrueba Validacion;

        private string FormatoFecha = "yyyy/mm/dd";

        private string FechaHoy = DateTime.Now.Date.ToString();

        public ReservaRepository(ReservaRestaurantContext context)
        {
            _restaurantContext = context;
            Validacion = new ValidacionClasePrueba();
        }

        private DateTime FormatoFechas(string fecha)
        {
            var fecharecibida = DateTime.Parse(fecha).Date;
            var formato = $"{fecharecibida.Year}-{fecharecibida.Month}-{fecharecibida.Day}";
            return DateTime.Parse(formato);
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
                FechaReserva = FormatoFechas(reserva.FechaReserva.Date.ToString()),
                FechaAlta = FormatoFechas(FechaHoy),
                FechaModificacion = FormatoFechas(FechaHoy),
                Estado = ""
            };

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
            var FechaConsulta = FormatoFechas(modificacion.FechaReserva.ToString());

            try
            {
                var ReservaModificar = await _restaurantContext.Reservas.
                    FirstOrDefaultAsync(r => r.Dni == modificacion.Dni
                    && r.FechaReserva == modificacion.FechaReserva
                    && r.IdRangoReserva == modificacion.IdRangoReserva
                    );
                //Tengo que agregar mas parametros para buscar la reserva y que los datos que se modifican
                //sean otras propiedades
                if(ReservaModificar != null )
                {
                    ReservaModificar.FechaReserva = FormatoFechas(modificacion.FechaModificacion.ToString());
                    ReservaModificar.IdRangoReserva = modificacion.IdRangoModificacion;
                    ReservaModificar.CantidadPersonas = modificacion.CantidadPersonasModificacion;
                    ReservaModificar.FechaModificacion = FormatoFechas(FechaHoy);

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
                var respuesta = new Respuesta();
                respuesta.Estado = false;
                respuesta.Mensaje.Clear();
                respuesta.Mensaje.Append("No se encontro reserva");
                return respuesta;
            }
            catch(Exception ex)
            {
                var respuesta = new Respuesta();
                respuesta.Estado = false;
                respuesta.Mensaje.Clear();
                respuesta.Mensaje.Append("No se encontro reserva");
                return respuesta;
            }
            return null;
        }

        public async Task<Respuesta> CancelarNewReservaAsync(CancelarDTO cancelar)
        {
            //Se bussca reserva para cancelar
            var ReservaCancelar = await _restaurantContext.Reservas.
                FirstOrDefaultAsync(r => r.Dni == cancelar.Dni 
                && r.FechaReserva == cancelar.FechaReserva
                && r.IdRangoReserva == cancelar.IdRangoReserva
                && r.Estado == "CONFIRMADO");

            Respuesta respuesta = new Respuesta();

            if(ReservaCancelar != null)
            {
                ReservaCancelar.Estado = "CANCELADO";

                await _restaurantContext.SaveChangesAsync();

                respuesta.Estado = true;
                respuesta.Mensaje.Clear();
                respuesta.Mensaje.Append("Se canceló la reserva.");
                return respuesta;
            }
            //No se encontro la reserva en la bdd
            respuesta.Estado = false;
            respuesta.Mensaje.Append("No se encontró la reserva.");
            return respuesta;
        }
    }
}
