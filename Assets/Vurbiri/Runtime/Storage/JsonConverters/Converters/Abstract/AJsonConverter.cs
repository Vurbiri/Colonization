using Newtonsoft.Json;
using System;

namespace Vurbiri
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

        public override bool CanConvert(Type objectType) => objectType != null && _type.IsAssignableFrom(objectType);

        protected virtual void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            throw Errors.NotSupportedWrite(_type);
        }
    }

    public abstract class AJsonConverter<TW, TR> : JsonConverter
    {
        protected readonly Type _writeType = typeof(TW), _readType = typeof(TR);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw Errors.NotSupportedRead(_readType);
        }

        sealed public override void WriteJson(JsonWriter writer, object obj, JsonSerializer serializer)
        {
            if (obj is not TW value)
                throw Errors.JsonSerialization(_writeType);

            WriteJson(writer, value, serializer);
        }

        public override bool CanConvert(Type objectType) => objectType != null && (_writeType.IsAssignableFrom(objectType) || _readType.IsAssignableFrom(objectType));

        protected virtual void WriteJson(JsonWriter writer, TW value, JsonSerializer serializer)
        {
            throw Errors.NotSupportedWrite(_writeType);
        }
    }
}
