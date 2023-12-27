using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Repository.Interface
{
    public interface ICalendarioSemanalRepository
    {
        public Task<List<CalendarioInfo>> GetCalendarioSemanal();
    }

    public class CalendarioInfo
    {
        public string Fecha { get; set; }
        public string Dia { get; set; }
        public List<RangoReservaInfo> Rangos { get; set; }
    }

    public class RangoReservaInfo
    {
        public string Rango { get; set; }
        public ReservaInfo Reserva { get; set; }
    }

    public class ReservaInfo
    {
        public int Ocupados { get; set; }
        public int Libres { get; set; }
        public int TotalCupos { get; set; }
    }
}
