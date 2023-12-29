namespace RestaurantApi.Domain.DTO
{
    public class ModificacionDTO
    {
        public string Dni { get; set; }
        public DateTime FechaReserva { get; set; }
        public int IdRangoReserva { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int IdRangoModificacion { get; set; }
        public int CantidadPersonasModificacion { get; set; }

    }
}
