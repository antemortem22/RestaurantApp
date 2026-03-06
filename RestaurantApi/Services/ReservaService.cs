using RestaurantApi.Domain.Common;
using RestaurantApi.Domain.Constants;
using RestaurantApi.Domain.DTO;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Repository.Interface;
using RestaurantApi.Services.Interface;

namespace RestaurantApi.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;
        private readonly IReservaValidator _validator;

        public ReservaService(IReservaRepository repository, IReservaValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<OperationResult> AddReservaAsync(ReservaDTO reserva)
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

            var validation = await _validator.ValidateReservaAsync(newReserva);
            if (!validation.Success)
            {
                return validation;
            }

            newReserva.Estado = ReservaEstado.Confirmado;
            await _repository.AddAsync(newReserva);

            var rows = await _repository.SaveChangesAsync();
            if (rows <= 0)
            {
                return OperationResult.Fail("Error en la api: no se agrego la reserva.");
            }

            return validation;
        }

        public async Task<OperationResult> ModificarReservaAsync(ModificacionDTO modificacion)
        {
            var reservaModificar = await _repository.GetByCriteriaAsync(
                modificacion.Dni,
                modificacion.FechaReserva,
                modificacion.IdRangoReserva);

            if (reservaModificar == null)
            {
                return OperationResult.Fail("No se encontro reserva.");
            }

            reservaModificar.FechaReserva = (modificacion.FechaModificacion ?? modificacion.FechaReserva).Date;
            reservaModificar.IdRangoReserva = modificacion.IdRangoModificacion;
            reservaModificar.CantidadPersonas = modificacion.CantidadPersonasModificacion;
            reservaModificar.FechaModificacion = DateTime.Today;

            var validation = await _validator.ValidateModificacionAsync(reservaModificar);
            if (!validation.Success)
            {
                return validation;
            }

            var rows = await _repository.SaveChangesAsync();
            if (rows <= 0)
            {
                return OperationResult.Fail("Error en la api: no se guardo la modificacion.");
            }

            return validation;
        }

        public async Task<OperationResult> CancelarReservaAsync(CancelarDTO cancelar)
        {
            var reservaCancelar = await _repository.GetConfirmedByCriteriaAsync(
                cancelar.Dni,
                cancelar.FechaReserva,
                cancelar.IdRangoReserva);

            if (reservaCancelar == null)
            {
                return OperationResult.Fail("No se encontro la reserva.");
            }

            reservaCancelar.Estado = ReservaEstado.Cancelado;

            var rows = await _repository.SaveChangesAsync();
            if (rows <= 0)
            {
                return OperationResult.Fail("Error en la api: no se pudo cancelar la reserva.");
            }

            return OperationResult.Ok("Se cancelo la reserva.");
        }
    }
}
