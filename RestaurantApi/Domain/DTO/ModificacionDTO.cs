using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Domain.DTO
{
    public class ModificacionDTO
    {
        [Required, RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener 7 u 8 digitos numericos.")]
        public string Dni { get; set; } = string.Empty;

        public DateTime FechaReserva { get; set; }

        [Range(1, 3, ErrorMessage = "El IdRangoReserva debe ser 1 (Almuerzo), 2 (Merienda) o 3 (Cena).")]
        public int IdRangoReserva { get; set; }

        [Required]
        public DateTime? FechaModificacion { get; set; }

        [Range(1, 3, ErrorMessage = "El IdRangoModificacion debe ser 1 (Almuerzo), 2 (Merienda) o 3 (Cena).")]
        public int IdRangoModificacion { get; set; }

        [Range(1, 50)]
        public int CantidadPersonasModificacion { get; set; }
    }
}
