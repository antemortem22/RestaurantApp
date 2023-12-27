using Microsoft.EntityFrameworkCore;
using RestaurantApi.Repository.Interface;
using System.Globalization;

namespace RestaurantApi.Repository
{
    public class CalendarioSemanalRepository : ICalendarioSemanalRepository
    {
        private readonly ReservaRestaurantContext _restaurantContext;

        public CalendarioSemanalRepository(ReservaRestaurantContext context)
        {
            _restaurantContext = context;
        }

        public async Task<List<CalendarioInfo>> GetCalendarioSemanal()
        {
            var fechasProximos7Dias = Enumerable.Range(0, 7)
            .Select(i => DateTime.Now.Date.AddDays(i))
            .ToList();

            var calendario = new List<CalendarioInfo>();

            foreach (var fecha in fechasProximos7Dias)
            {
                //obtiene el actual dia de la semana
                var diaSemana = fecha.ToString("dddd", CultureInfo.CurrentCulture);

                var infoDia = new CalendarioInfo
                {
                    Fecha = fecha.ToString("yyyy-MM-dd"),
                    Dia = diaSemana,
                    Rangos = await ObtenerInfoRangosAsync(fecha)
                };

                calendario.Add(infoDia);
            }

            return calendario;
        }

        private async Task<List<RangoReservaInfo>> ObtenerInfoRangosAsync(DateTime fecha)
        {
            var rangos = await _restaurantContext.RangoReservas.ToListAsync();
            var infoRangos = new List<RangoReservaInfo>();

            foreach (var rango in rangos)
            {
                var reservasEnRango = await _restaurantContext.Reservas
                    .Where(r => r.FechaReserva.Date == fecha.Date && r.IdRangoReserva == rango.IdRangoReserva)
                    .ToListAsync();

                var rangoInfo = new RangoReservaInfo
                {
                    Rango = rango.Descripcion,
                    Reserva = new ReservaInfo
                    {
                        Ocupados = reservasEnRango.Count(r => r.Estado == "Confirmado"),
                        Libres = rango.Cupo - reservasEnRango.Count(r => r.Estado == "Confirmado"),
                        TotalCupos = rango.Cupo
                    }
                };

                infoRangos.Add(rangoInfo);
            }

            return infoRangos;
        }
    }
}
