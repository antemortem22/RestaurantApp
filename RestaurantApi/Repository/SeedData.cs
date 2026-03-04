using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Repository
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ReservaRestaurantContext context)
        {
            // Aplica migraciones pendientes.
            await context.Database.MigrateAsync();

            await SeedRangosAsync(context);
            await SeedReservasAsync(context);
        }

        private static async Task SeedRangosAsync(ReservaRestaurantContext context)
        {
            if (await context.RangoReservas.AnyAsync())
                return;

            context.RangoReservas.AddRange(
                new RangoReserva { Descripcion = "Almuerzo", Cupo = 20 },
                new RangoReserva { Descripcion = "Merienda", Cupo = 15 },
                new RangoReserva { Descripcion = "Cena", Cupo = 25 }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedReservasAsync(ReservaRestaurantContext context)
        {
            if (await context.Reservas.AnyAsync())
                return;

            var rangos = await context.RangoReservas.AsNoTracking().ToListAsync();
            if (rangos.Count == 0)
                return;

            var random = new Random(42);
            var nombres = new[] { "Ana", "Juan", "Sofia", "Lucas", "Pedro", "Mica", "Valen", "Carla" };
            var apellidos = new[] { "Perez", "Gomez", "Diaz", "Lopez", "Torres", "Sosa", "Ramos", "Ibarra" };

            var reservas = new List<Reserva>();
            var hoy = DateTime.Today;

            for (int dia = 0; dia < 7; dia++)
            {
                var fechaReserva = hoy.AddDays(dia);

                foreach (var rango in rangos)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var idx = random.Next(nombres.Length);
                        var estado = random.Next(100) < 80 ? "CONFIRMADO" : "CANCELADO";

                        reservas.Add(new Reserva
                        {
                            CodReserva = $"R-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
                            NombrePersona = nombres[idx],
                            ApellidoPersona = apellidos[idx],
                            Dni = random.Next(30000000, 49999999).ToString(),
                            Mail = $"demo{dia}{rango.IdRangoReserva}{i}@mail.com",
                            Celular = "1122334455",
                            FechaReserva = fechaReserva,
                            IdRangoReserva = rango.IdRangoReserva,
                            CantidadPersonas = random.Next(1, 5),
                            FechaAlta = hoy,
                            FechaModificacion = hoy,
                            Estado = estado
                        });
                    }
                }
            }

            await context.Reservas.AddRangeAsync(reservas);
            await context.SaveChangesAsync();
        }
    }
}
