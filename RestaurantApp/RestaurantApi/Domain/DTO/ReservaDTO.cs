namespace RestaurantApi.Domain.DTO
{
    public class ReservaDTO
    {
        public string NombrePersona { get; set; }
        public string ApellidoPersona { get; set; }
        public string Dni { get; set; }
        public string Mail { get; set; }
        public string Celular { get; set; }
        public DateTime FechaReserva { get; set; }
        public int IdRangoReserva { get; set; }
        public int CantidadPersonas { get; set; }
    }
}
