using System.ComponentModel.DataAnnotations;
namespace RestaurantApi.Domain.DTO
{
    public class ModificacionDTO
    {
        [Required, RegularExpression(@"^\d{7,8}$")]
        public string Dni { get; set; } = string.Empty;

        public DateTime FechaReserva { get; set; }

        [Range(1, int.MaxValue)]
        public int IdRangoReserva { get; set; }

        [Required]
        public DateTime? FechaModificacion { get; set; }

        [Range(1, int.MaxValue)]
        public int IdRangoModificacion { get; set; }

        [Range(1, 50)]
        public int CantidadPersonasModificacion { get; set; }

    }
}
