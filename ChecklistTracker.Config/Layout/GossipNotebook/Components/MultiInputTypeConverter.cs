using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChecklistTracker.Config.Layout.GossipNotebook.Components
{
    internal abstract class MultiInputTypeConverter<T, C> : JsonConverter<T>
        where T : class
        // A bit goofy but we need a different type to convert to in order to prevent infinite loops.
        // C generally just extend T without adding any new properties.
        where C : T
    {
        protected virtual T? FromArray(double[] value) => throw new JsonException($"Parsing {typeof(T)} from array is not supported");
        protected virtual T? FromString(string value) => throw new JsonException($"Parsing {typeof(T)} from string is not supported");

        private bool IsEnabled = true;

        public override bool CanConvert(Type typeToConvert)
        {
            return IsEnabled && base.CanConvert(typeToConvert);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var array = JsonSerializer.Deserialize<double[]>(ref reader, options);
                if (array == null)
                {
                    return null;
                }
                return FromArray(array);
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (str == null)
                {
                    return null;
                }
                return FromString(str);
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                return JsonSerializer.Deserialize<C>(ref reader, options);
            }

            throw new JsonException($"Unexpected token parsing {typeof(T)}. Got {reader.TokenType}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
