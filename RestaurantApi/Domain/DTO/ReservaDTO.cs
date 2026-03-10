using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Domain.DTO
{
    public class ReservaDTO
    {
        [Required, StringLength(50, MinimumLength = 2)]
        public string NombrePersona { get; set; } = string.Empty;

        [Required, StringLength(50, MinimumLength = 2)]
        public string ApellidoPersona { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener 7 u 8 digitos numericos.")]
        public string Dni { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(50)]
        public string Mail { get; set; } = string.Empty;

        [Required, RegularExpression(@"^[0-9+\-\s]{8,20}$", ErrorMessage = "El celular debe contener entre 8 y 20 caracteres validos.")]
        public string Celular { get; set; } = string.Empty;

        public DateTime FechaReserva { get; set; }

        [Range(1, 3, ErrorMessage = "El IdRangoReserva debe ser 1 (Almuerzo), 2 (Merienda) o 3 (Cena).")]
        public int IdRangoReserva { get; set; }

        [Range(1, 50)]
        public int CantidadPersonas { get; set; }
    }
}
