using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestaurantApi.Serialization
{
    public class ArgentinaDateTimeJsonConverter : JsonConverter<DateTime>
    {
        private static readonly CultureInfo ArgentineCulture = CultureInfo.GetCultureInfo("es-AR");
        private static readonly string[] AcceptedFormats =
        {
            "dd/MM/yyyy",
            "d/M/yyyy",
            "dd/MM/yyyy HH:mm:ss",
            "d/M/yyyy H:m:s"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Formato de fecha invalido. Use dd/MM/yyyy.");
            }

            var rawValue = reader.GetString();
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                throw new JsonException("La fecha no puede estar vacia. Use dd/MM/yyyy.");
            }

            if (DateTime.TryParseExact(rawValue, AcceptedFormats, ArgentineCulture, DateTimeStyles.None, out var exactDate))
            {
                return exactDate;
            }

            throw new JsonException("Formato de fecha invalido. Use dd/MM/yyyy.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("dd/MM/yyyy", ArgentineCulture));
        }
    }

    public class NullableArgentinaDateTimeJsonConverter : JsonConverter<DateTime?>
    {
        private readonly ArgentinaDateTimeJsonConverter _baseConverter = new();

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            return _baseConverter.Read(ref reader, typeof(DateTime), options);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
                return;
            }

            _baseConverter.Write(writer, value.Value, options);
        }
    }
}
