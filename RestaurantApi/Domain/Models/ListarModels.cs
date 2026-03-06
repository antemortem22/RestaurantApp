namespace RestaurantApi.Domain.Models
{
    // Modelos de respuesta usados por los endpoints de calendario y disponibilidad.
    public class TurnoInfo
    {
        public string Rango { get; set; } = string.Empty;
    }

    public class ListaTurnoInfo
    {
        public string Fecha { get; set; } = string.Empty;
        public string Dia { get; set; } = string.Empty;
        public List<TurnoInfo> Rangos { get; set; } = new();
    }

    public class CalendarioInfo
    {
        public string Fecha { get; set; } = string.Empty;
        public string Dia { get; set; } = string.Empty;
        public List<RangoReservaInfo> Rangos { get; set; } = new();
    }

    public class RangoReservaInfo
    {
        public string Rango { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }

    public class CalendarioResponse
    {
        public List<CalendarioReserva> Calendarios { get; set; } = new();
    }

    public class CalendarioReserva
    {
        public string Fecha { get; set; } = string.Empty;
        public string Dia { get; set; } = string.Empty;
        public List<RangoReservaCalendario> Rangos { get; set; } = new();
    }

    public class RangoReservaCalendario
    {
        public string Rango { get; set; } = string.Empty;
        public ReservaCalendario Reserva { get; set; } = new();
    }

    public class ReservaCalendario
    {
        public int Ocupados { get; set; }
        public int Libres { get; set; }
        public int TotalCupos { get; set; }
    }
}

