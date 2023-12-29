namespace RestaurantApi.Domain.DTO
{
    public class ModificacionDTO
    {
        public string CodReserva { get; set; }
        public DateTime FechaReserva { get; set; }
        public int IdRangoReserva { get; set; }
        public int CantidadPersonas { get; set; }

    }
}
