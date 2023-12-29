namespace RestaurantApi.Domain.DTO
{
    public class CancelarDTO
    {
        public string Dni { get; set; }
        public DateTime FechaReserva { get; set; }
        public int IdRangoReserva { get; set; }
    }
}
