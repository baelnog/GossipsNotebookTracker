using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ChecklistTracker.Layout
{
    /// <summary>
    /// Polymorphic Json Converter
    /// Works around limitations in current .NET polymorphic support
    /// https://github.com/dotnet/runtime/issues/72604#issuecomment-1440708052
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PolymorphicJsonConverter<T> : JsonConverter<T>
    {
        private readonly string discriminatorPropName;
        private readonly Func<Type, string> getDiscriminator;
        private readonly IReadOnlyDictionary<string, Type> discriminatorToSubtype;

        public PolymorphicJsonConverter(
            string typeDiscriminatorPropertyName,
            Func<Type, string> getDiscriminatorForSubtype,
            IEnumerable<Type> subtypes)
        {
            discriminatorPropName = typeDiscriminatorPropertyName;
            getDiscriminator = getDiscriminatorForSubtype;
            discriminatorToSubtype = subtypes.ToDictionary(getDiscriminator, t => t);
        }

        public override bool CanConvert(Type typeToConvert)
            => typeof(T).IsAssignableFrom(typeToConvert);

        // When a custom converter is defined for a JsonSerializerOptions instance,
        // you can't use the options to get the JsonTypeInfo for any types the
        // converter can convert, so unfortunately we have to create a copy with
        // the converters removed.
        JsonSerializerOptions? originalOptions = null;
        JsonSerializerOptions? optionsWithoutConverters = null;
        JsonTypeInfo getTypeInfo(Type t, JsonSerializerOptions givenOpts)
        {
            if (optionsWithoutConverters is null)
            {
                originalOptions = givenOpts;
                optionsWithoutConverters = new(givenOpts);
                optionsWithoutConverters.Converters.Clear();
            }

            if (originalOptions != givenOpts)
            {
                throw new Exception(
                    $"A {typeof(PolymorphicJsonConverter<>).Name} instance cannot " +
                    $"be used in multiple {nameof(JsonSerializerOptions)} instances!");
            }

            return optionsWithoutConverters.GetTypeInfo(t);
        }

        public override T Read(
            ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);

            JsonElement root = doc.RootElement;
            JsonElement typeField = root.GetProperty(discriminatorPropName);

            if (typeField.GetString() is not string typeName)
            {
                throw new JsonException(
                    $"Could not find string property {discriminatorPropName} " +
                    $"when trying to deserialize {typeof(T).Name}");
            }

            if (!discriminatorToSubtype.TryGetValue(typeName.ToLower(), out Type? type))
            {
                throw new JsonException($"Unknown type: {typeName}");
            }

            JsonTypeInfo info = getTypeInfo(type, options);

            T instance = (T)info.CreateObject!();

            foreach (var p in info.Properties)
            {
                if (p.Set is null) continue;

                if (!root.TryGetProperty(p.Name, out JsonElement propValue))
                {
                    if (p.IsRequired)
                    {
                        throw new JsonException($"Required property {p.Name} was not found.");
                    }
                    else
                    {
                        continue;
                    }
                }

                p.Set(instance, propValue.Deserialize(p.PropertyType, options));
            }

            return instance;
        }

        public override void Write(
            Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            Type type = value!.GetType();

            if (type == typeof(T))
            {
                throw new NotSupportedException(
                    $"Cannot serialize an instance of type {typeof(T)}, only its subtypes.");
            }

            writer.WriteStartObject();

            writer.WriteString(discriminatorPropName, getDiscriminator(type));

            JsonTypeInfo info = getTypeInfo(type, options);

            foreach (var p in info.Properties)
            {
                if (p.Get is null) continue;

                writer.WritePropertyName(p.Name);
                object? pVal = p.Get(value);
                JsonSerializer.Serialize(writer, pVal, options);
            }

            writer.WriteEndObject();
        }
    }
}
