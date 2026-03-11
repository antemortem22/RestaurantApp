namespace RestaurantBlazor.Models;

public class CalendarioResponseDto
{
    public List<CalendarioReservaDto> Calendarios { get; set; } = new();
}

public class CalendarioReservaDto
{
    public string Fecha { get; set; } = string.Empty;
    public string Dia { get; set; } = string.Empty;
    public List<RangoReservaCalendarioDto> Rangos { get; set; } = new();
}

public class RangoReservaCalendarioDto
{
    public string Rango { get; set; } = string.Empty;
    public ReservaCalendarioDto Reserva { get; set; } = new();
}

public class ReservaCalendarioDto
{
    public int Ocupados { get; set; }
    public int Libres { get; set; }
    public int TotalCupos { get; set; }
}
