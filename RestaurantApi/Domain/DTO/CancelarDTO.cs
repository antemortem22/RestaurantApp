using System.ComponentModel.DataAnnotations;
namespace RestaurantApi.Domain.DTO
{
    public class CancelarDTO
    {
        [Required, RegularExpression(@"^\d{7,8}$")]
        public string Dni { get; set; } = string.Empty;
        public DateTime FechaReserva { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El IdRangoReserva debe ser mayor o igual a 1.")]
        public int IdRangoReserva { get; set; }
    }
}
