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

        public async Task<List<CalendarioInfo>> GetCalendarioSemanalAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);

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

        public async Task<List<CalendarioInfo>> GetCanceladosSemanalAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);

            var turnosCancelados = await _restaurantContext.Reservas
                .Where(r => r.Estado == "CANCELADO" && fechasProximos7Dias.Contains(r.FechaReserva.Date))
                .GroupBy(r => new { r.FechaReserva.Date, r.IdRangoReserva })
                .Select(group => new CalendarioInfo
                {
                    Fecha = group.Key.Date.ToString("yyyy-MM-dd"),
                    Dia = group.Key.Date.ToString("dddd", CultureInfo.CurrentCulture),
                    Rangos = group.Select(r => new RangoReservaInfo
                    {
                        Rango = r.IdRangoReservaNavigation.Descripcion,
                        Cancelado = true,
                        Confirmado = false
                    }).ToList()
                })
                .ToListAsync();

            return turnosCancelados;
        }

        public async Task<List<CalendarioInfo>> GetConfirmadosSemanalAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);

            var turnosConfirmados = await _restaurantContext.Reservas
                .Where(r => r.Estado == "CONFIRMADO" && fechasProximos7Dias.Contains(r.FechaReserva.Date))
                .GroupBy(r => new { r.FechaReserva.Date, r.IdRangoReserva })
                .Select(group => new CalendarioInfo
                {
                    Fecha = group.Key.Date.ToString("yyyy-MM-dd"),
                    Dia = group.Key.Date.ToString("dddd", CultureInfo.CurrentCulture),
                    Rangos = group.Select(r => new RangoReservaInfo
                    {
                        Rango = r.IdRangoReservaNavigation.Descripcion,
                        Cancelado = false,
                        Confirmado = true
                    }).ToList()
                })
                .ToListAsync();

            return turnosConfirmados;
        }

        public async Task<List<RangoReservaInfo>> GetTurnosSinCupoAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);

            var turnosSinCupo = await _restaurantContext.RangoReservas
                                    .Where(r => r.Reservas.Any() && r.Reservas.Sum(reserva => reserva.CantidadPersonas) == 0)
                                    .Select(r => new RangoReservaInfo
                                    {
                                        IdRangoReserva = r.IdRangoReserva,
                                        Rango = r.Descripcion, // Ajustar para reflejar la estructura de la base de datos
                                    })
                                    .ToListAsync();


            foreach (var fecha in fechasProximos7Dias)
            {
                var reservasPorFecha = _restaurantContext.Reservas
                    .Where(r => r.FechaReserva.Date == fecha.Date && turnosSinCupo.Any(t => t.IdRangoReserva == r.IdRangoReserva))
                    .GroupBy(r => r.IdRangoReserva)
                    .ToDictionary(group => group.Key, group => group.Sum(r => r.CantidadPersonas));

                foreach (var turno in turnosSinCupo)
                {
                    if (reservasPorFecha.ContainsKey(turno.IdRangoReserva))
                    {
                        turno.Reserva = new ReservaInfo
                        {
                            Ocupados = reservasPorFecha[turno.IdRangoReserva],
                            Fecha = fecha.ToString("yyyy-MM-dd"),
                        };
                    }
                }
            }

            return turnosSinCupo;
        }

        public async Task<List<CalendarioInfo>> GetTurnosDisponiblesPorFechaAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);

            var turnosDisponibles = new List<CalendarioInfo>();

            foreach (var fecha in fechasProximos7Dias)
            {
                var turnosPorFecha = await _restaurantContext.RangoReservas
                    .Select(r => new
                    {
                        r.IdRangoReserva,
                        r.Descripcion,
                        CupoTotal = r.Cupo,
                        ReservasOcupadas = r.Reservas
                            .Where(reserva => reserva.FechaReserva.Date == fecha.Date)
                            .Sum(reserva => reserva.CantidadPersonas)
                    })
                    .ToListAsync();

                var calendarioInfo = new CalendarioInfo
                {
                    Fecha = fecha.ToString("yyyy-MM-dd"),
                    Dia = fecha.ToString("dddd", CultureInfo.CurrentCulture),
                    Rangos = turnosPorFecha
                        .Select(turno => new RangoReservaInfo
                        {
                            Rango = $"{turno.Descripcion} ({turno.ReservasOcupadas}/{turno.CupoTotal})",
                            Reserva = new ReservaInfo
                            {
                                
                                Libres = Math.Max(0, turno.CupoTotal - turno.ReservasOcupadas),
                                TotalCupos = turno.CupoTotal
                            }
                        })
                        .ToList()
                };

                turnosDisponibles.Add(calendarioInfo);
            }

            return turnosDisponibles;
        }


        /**********************************FUNCIONES*****************************/
        private async Task<List<DateTime>> ObtenerFechasProximosDiasAsync(int dias)
        {
            return await Task.Run(() =>
            {
                return Enumerable.Range(0, dias)
                    .Select(i => DateTime.Now.Date.AddDays(i))
                    .ToList();
            });
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
                        Ocupados = reservasEnRango.Count(r => r.Estado == "CONFIRMADO"),
                        Libres = rango.Cupo - reservasEnRango.Count(r => r.Estado == "CONFIRMADO"),
                        TotalCupos = rango.Cupo
                    }
                };

                infoRangos.Add(rangoInfo);
            }

            return infoRangos;
        }
    }
}
