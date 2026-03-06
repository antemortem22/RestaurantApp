using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.Constants;
using RestaurantApi.Domain.Models;
using RestaurantApi.Repository.Interface;
using System.Globalization;

namespace RestaurantApi.Repository
{
    public class CalendarioSemanalRepository : ICalendarioSemanalRepository
    {
        private static readonly CultureInfo ArgentineCulture = CultureInfo.GetCultureInfo("es-AR");
        private readonly ReservaRestaurantContext _restaurantContext;

        public CalendarioSemanalRepository(ReservaRestaurantContext context)
        {
            _restaurantContext = context;
        }

        public async Task<List<CalendarioResponse>> GetCalendarioSemanalAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);
            var calendario = new List<CalendarioReserva>();

            foreach (var fecha in fechasProximos7Dias)
            {
                var diaSemana = fecha.ToString("dddd", ArgentineCulture);

                var turnosPorFecha = await _restaurantContext.RangoReservas
                    .Select(r => new
                    {
                        r.Descripcion,
                        CupoTotal = r.Cupo,
                        ReservasOcupadas = r.Reservas
                            .Where(reserva => reserva.FechaReserva.Date == fecha.Date)
                            .Sum(reserva => reserva.CantidadPersonas)
                    })
                    .ToListAsync();

                var calendarioReserva = new CalendarioReserva
                {
                    Fecha = fecha.ToString("dd/MM/yyyy", ArgentineCulture),
                    Dia = diaSemana,
                    Rangos = turnosPorFecha
                        .Select(turno => new RangoReservaCalendario
                        {
                            Rango = $"{turno.Descripcion} ({turno.ReservasOcupadas}/{turno.CupoTotal})",
                            Reserva = new ReservaCalendario
                            {
                                Ocupados = turno.ReservasOcupadas,
                                Libres = Math.Max(0, turno.CupoTotal - turno.ReservasOcupadas),
                                TotalCupos = turno.CupoTotal
                            }
                        })
                        .ToList()
                };

                calendario.Add(calendarioReserva);
            }

            var calendarioResponse = new CalendarioResponse
            {
                Calendarios = calendario
            };

            return new List<CalendarioResponse> { calendarioResponse };
        }

        public async Task<List<CalendarioInfo>> GetCanceladosSemanalAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);
            var calendario = new List<CalendarioInfo>();

            foreach (var fecha in fechasProximos7Dias)
            {
                var diaSemana = fecha.ToString("dddd", ArgentineCulture);

                var turnosCanceladosPorFecha = await _restaurantContext.RangoReservas
                    .SelectMany(r => r.Reservas
                        .Where(reserva => reserva.FechaReserva.Date == fecha.Date && reserva.Estado == ReservaEstado.Cancelado)
                        .Select(reserva => new
                        {
                            r.Descripcion,
                            reserva.NombrePersona,
                            reserva.ApellidoPersona,
                            reserva.Estado
                        })
                    )
                    .ToListAsync();

                if (turnosCanceladosPorFecha.Any())
                {
                    var calendarioInfo = new CalendarioInfo
                    {
                        Fecha = fecha.ToString("dd/MM/yyyy", ArgentineCulture),
                        Dia = diaSemana,
                        Rangos = turnosCanceladosPorFecha
                            .Select(turno => new RangoReservaInfo
                            {
                                Nombre = turno.NombrePersona,
                                Apellido = turno.ApellidoPersona,
                                Rango = turno.Descripcion,
                                Estado = turno.Estado
                            })
                            .ToList()
                    };

                    calendario.Add(calendarioInfo);
                }
            }

            return calendario;
        }

        public async Task<List<CalendarioInfo>> GetConfirmadosSemanalAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);
            var calendario = new List<CalendarioInfo>();

            foreach (var fecha in fechasProximos7Dias)
            {
                var diaSemana = fecha.ToString("dddd", ArgentineCulture);

                var turnosConfirmadosPorFecha = await _restaurantContext.RangoReservas
                    .SelectMany(r => r.Reservas
                        .Where(reserva => reserva.FechaReserva.Date == fecha.Date && reserva.Estado == ReservaEstado.Confirmado)
                        .Select(reserva => new
                        {
                            r.Descripcion,
                            reserva.NombrePersona,
                            reserva.ApellidoPersona,
                            reserva.Estado
                        })
                    )
                    .ToListAsync();

                if (turnosConfirmadosPorFecha.Any())
                {
                    var calendarioInfo = new CalendarioInfo
                    {
                        Fecha = fecha.ToString("dd/MM/yyyy", ArgentineCulture),
                        Dia = diaSemana,
                        Rangos = turnosConfirmadosPorFecha
                            .Select(turno => new RangoReservaInfo
                            {
                                Nombre = turno.NombrePersona,
                                Apellido = turno.ApellidoPersona,
                                Rango = turno.Descripcion,
                                Estado = turno.Estado
                            })
                            .ToList()
                    };

                    calendario.Add(calendarioInfo);
                }
            }

            return calendario;
        }

        public async Task<List<ListaTurnoInfo>> GetTurnosSinCupoAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);
            var turnosSinCupo = new List<ListaTurnoInfo>();

            foreach (var fecha in fechasProximos7Dias)
            {
                var diaSemana = fecha.ToString("dddd", ArgentineCulture);

                var turnosPorFecha = await _restaurantContext.RangoReservas
                    .Select(r => new
                    {
                        r.Descripcion,
                        CupoTotal = r.Cupo,
                        ReservasOcupadas = r.Reservas
                            .Where(reserva => reserva.FechaReserva.Date == fecha.Date)
                            .Sum(reserva => reserva.CantidadPersonas)
                    })
                    .ToListAsync();

                var turnosSinCupoPorFecha = turnosPorFecha
                    .Where(turno => turno.ReservasOcupadas >= turno.CupoTotal)
                    .Select(turno => new ListaTurnoInfo
                    {
                        Fecha = fecha.ToString("dd/MM/yyyy", ArgentineCulture),
                        Dia = diaSemana,
                        Rangos = new List<TurnoInfo>
                        {
                            new TurnoInfo
                            {
                                Rango = $"{turno.Descripcion} ({turno.ReservasOcupadas}/{turno.CupoTotal})"
                            }
                        }
                    })
                    .ToList();

                if (turnosSinCupoPorFecha.Any())
                {
                    turnosSinCupo.AddRange(turnosSinCupoPorFecha);
                }
            }

            return turnosSinCupo;
        }

        public async Task<List<ListaTurnoInfo>> GetTurnosDisponiblesPorFechaAsync()
        {
            var fechasProximos7Dias = await ObtenerFechasProximosDiasAsync(7);
            var turnosDisponibles = new List<ListaTurnoInfo>();

            foreach (var fecha in fechasProximos7Dias)
            {
                var diaSemana = fecha.ToString("dddd", ArgentineCulture);

                var turnosPorFecha = await _restaurantContext.RangoReservas
                    .Select(r => new
                    {
                        r.Descripcion,
                        CupoTotal = r.Cupo,
                        ReservasOcupadas = r.Reservas
                            .Where(reserva => reserva.FechaReserva.Date == fecha.Date)
                            .Sum(reserva => reserva.CantidadPersonas)
                    })
                    .ToListAsync();

                var turnosDisponiblesPorFecha = turnosPorFecha
                    .Where(turno => turno.ReservasOcupadas < turno.CupoTotal)
                    .Select(turno => new ListaTurnoInfo
                    {
                        Fecha = fecha.ToString("dd/MM/yyyy", ArgentineCulture),
                        Dia = diaSemana,
                        Rangos = new List<TurnoInfo>
                        {
                            new TurnoInfo
                            {
                                Rango = $"{turno.Descripcion} ({turno.ReservasOcupadas}/{turno.CupoTotal})"
                            }
                        }
                    })
                    .ToList();

                if (turnosDisponiblesPorFecha.Any())
                {
                    turnosDisponibles.AddRange(turnosDisponiblesPorFecha);
                }
            }

            return turnosDisponibles;
        }

        // Genera una ventana de fechas desde hoy para reutilizar en consultas semanales.
        private static async Task<List<DateTime>> ObtenerFechasProximosDiasAsync(int dias)
        {
            return await Task.Run(() => Enumerable.Range(0, dias)
                .Select(i => DateTime.Now.Date.AddDays(i))
                .ToList());
        }
    }
}
