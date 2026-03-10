using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using RestaurantApi.Domain.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestaurantApi.Swagger
{
    public class DtoExamplesSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(ReservaDTO))
            {
                schema.Example = new OpenApiObject
                {
                    ["nombrePersona"] = new OpenApiString("Agostina"),
                    ["apellidoPersona"] = new OpenApiString("Perez"),
                    ["dni"] = new OpenApiString("30123456"),
                    ["mail"] = new OpenApiString("agos.perez@mail.com"),
                    ["celular"] = new OpenApiString("1133344455"),
                    ["fechaReserva"] = new OpenApiString("15/03/2026"),
                    ["idRangoReserva"] = new OpenApiInteger(1),
                    ["cantidadPersonas"] = new OpenApiInteger(2)
                };
                return;
            }

            if (context.Type == typeof(ModificacionDTO))
            {
                schema.Example = new OpenApiObject
                {
                    ["dni"] = new OpenApiString("30123456"),
                    ["fechaReserva"] = new OpenApiString("15/03/2026"),
                    ["idRangoReserva"] = new OpenApiInteger(1),
                    ["fechaModificacion"] = new OpenApiString("16/03/2026"),
                    ["idRangoModificacion"] = new OpenApiInteger(2),
                    ["cantidadPersonasModificacion"] = new OpenApiInteger(3)
                };
                return;
            }

            if (context.Type == typeof(CancelarDTO))
            {
                schema.Example = new OpenApiObject
                {
                    ["dni"] = new OpenApiString("30123456"),
                    ["fechaReserva"] = new OpenApiString("16/03/2026"),
                    ["idRangoReserva"] = new OpenApiInteger(2)
                };
            }
        }
    }
}
