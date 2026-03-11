namespace RestaurantBlazor.Models;

public class ReservaRequestDto
{
    public string NombrePersona { get; set; } = string.Empty;
    public string ApellidoPersona { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string Celular { get; set; } = string.Empty;

    // La API espera dd/MM/yyyy
    public string FechaReserva { get; set; } = string.Empty;
    public int IdRangoReserva { get; set; } = 1;
    public int CantidadPersonas { get; set; } = 1;
}

public class ModificacionRequestDto
{
    public string Dni { get; set; } = string.Empty;
    public string FechaReserva { get; set; } = string.Empty;
    public int IdRangoReserva { get; set; } = 1;
    public string FechaModificacion { get; set; } = string.Empty;
    public int IdRangoModificacion { get; set; } = 1;
    public int CantidadPersonasModificacion { get; set; } = 1;
}

public class CancelarRequestDto
{
    public string Dni { get; set; } = string.Empty;
    public string FechaReserva { get; set; } = string.Empty;
    public int IdRangoReserva { get; set; } = 1;
}

public class ApiMessageResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
