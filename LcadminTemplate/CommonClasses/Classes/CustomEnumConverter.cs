namespace CommonClasses
{
    // File: Converters/CustomEnumConverter.cs

    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class CustomEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public static string? ErrorMessage { get; private set; }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ErrorMessage = null;

            if (reader.TokenType == JsonTokenType.Null)
            {
                ErrorMessage = "State is not selected.";
                return default;
            }

            var value = reader.GetString();
            if (System.Enum.TryParse(typeof(T), value, true, out var result))
            {
                return (T)result;
            }

            ErrorMessage = "State is not selected.";
            return default;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
