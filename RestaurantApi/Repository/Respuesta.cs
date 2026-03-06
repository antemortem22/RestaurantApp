using System.Text;

namespace RestaurantApi.Repository
{
    public class Respuesta
    {
        public bool Estado { get; set; } = true;
        public StringBuilder Mensaje { get; } = new("Error en el campo: ");
    }
}
