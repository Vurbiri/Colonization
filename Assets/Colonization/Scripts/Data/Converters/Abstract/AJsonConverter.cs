//Assets\Colonization\Scripts\Data\Converters\Abstract\AJsonConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization.Data
{
    public abstract class AJsonConverter<T> : JsonConverter
    {
        protected readonly Type _type = typeof(T);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw Errors.NotSupportedRead(_type);
        }

        sealed public override void WriteJson(JsonWriter writer, object obj, JsonSerializer serializer)
        {
            if (obj is not T value)
                throw Errors.JsonSerialization(_type);

            WriteJson(writer, value, serializer);
        }

        public override bool CanConvert(Type objectType) => _type.IsAssignableFrom(objectType);

        protected virtual void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            throw Errors.NotSupportedWrite(_type);
        }
    }
}
