

namespace RestaurantApi.Repository.Interface
{
    public interface ICalendarioSemanalRepository
    {
        public Task<List<CalendarioInfo>> GetCalendarioSemanalAsync();
        public Task<List<CalendarioInfo>> GetCanceladosSemanalAsync();
        public Task<List<CalendarioInfo>> GetConfirmadosSemanalAsync();
        public Task<List<RangoReservaInfo>> GetTurnosSinCupoAsync();
        public Task<List<CalendarioInfo>> GetTurnosDisponiblesPorFechaAsync();
    }

    public class CalendarioInfo
    {
        public string Fecha { get; set; }
        public string Dia { get; set; } = string.Empty;
        public List<RangoReservaInfo> Rangos { get; set; } = new List<RangoReservaInfo>();
    }

    public class RangoReservaInfo
    {
        public int IdRangoReserva { get; set; }
        public string Rango { get; set; }
        public bool Cancelado { get; set; }
        public bool Confirmado { get; set; }
        public ReservaInfo Reserva { get; set; } = new ReservaInfo();
    }

    public class ReservaInfo
    {
        public int Ocupados { get; set; }
        public int Libres { get; set; }
        public int TotalCupos { get; set; }
        public string Fecha { get; set; }
       
    }


}
