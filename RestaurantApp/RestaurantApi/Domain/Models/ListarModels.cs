namespace RestaurantApi.Domain.Models
{

    //Clases para listar por disponibilidad
    public class TurnoInfo
    {
        public string Rango { get; set; }
    }
    public class ListaTurnoInfo
    {
        public string Fecha { get; set; }
        public string Dia { get; set; } = string.Empty;
        public List<TurnoInfo> Rangos { get; set; } = new List<TurnoInfo>();
    }


    //Clases para cancelados y confirmados
    public class CalendarioInfo
    {
        public string Fecha { get; set; }
        public string Dia { get; set; } = string.Empty;
        public List<RangoReservaInfo> Rangos { get; set; } = new List<RangoReservaInfo>();
    }

    public class RangoReservaInfo
    {

        public string Rango { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Estado { get; set; }

    }

    //Clases especifica para el formato del calendario
    public class CalendarioResponse
    {
        public List<CalendarioReserva> Calendarios { get; set; }
    }
    public class CalendarioReserva
    {
        public string Fecha { get; set; }
        public string Dia { get; set; } = string.Empty;
        public List<RangoReservaCalendario> Rangos { get; set; } = new List<RangoReservaCalendario>();
    }
    public class RangoReservaCalendario
    {
        public string Rango { get; set; }

        public ReservaCalendario Reserva { get; set; }
    }

    public class ReservaCalendario
    {
        public int Ocupados { get; set; }
        public int Libres { get; set; }
        public int TotalCupos { get; set; }

    }
}
