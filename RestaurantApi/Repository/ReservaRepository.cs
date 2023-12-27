﻿using Microsoft.EntityFrameworkCore;
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

            //Restar a los cupos por turno
            var rangoReserva = await _restaurantContext.RangoReservas.FindAsync(reserva.IdRangoReserva);

            /*if (rangoReserva != null)
            {
                // Restar la cantidad de personas del cupo
                rangoReserva.Cupo -= reserva.CantidadPersonas;

                // Guardar los cambios en la base de datos
                await _restaurantContext.SaveChangesAsync();
            }*/


            reserva.Estado.ToUpper();

            await _restaurantContext.Reservas.AddAsync(newReserva);

            int rows = await _restaurantContext.SaveChangesAsync();

            return rows > 0;
        }

    }
}
