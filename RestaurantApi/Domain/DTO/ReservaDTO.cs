using System.ComponentModel.DataAnnotations;
namespace RestaurantApi.Domain.DTO
{
    public class ReservaDTO
    {
        [Required, StringLength(50, MinimumLength = 2)]
        public string NombrePersona { get; set; } = string.Empty;

        [Required, StringLength(50, MinimumLength = 2)]
        public string ApellidoPersona { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{7,8}$")]
        public string Dni { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(50)]
        public string Mail { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Celular { get; set; } = string.Empty;

        public DateTime FechaReserva { get; set; }

        [Range(1, int.MaxValue)]
        public int IdRangoReserva { get; set; }

        [Range(1, 50)]
        public int CantidadPersonas { get; set; }
    }
}
