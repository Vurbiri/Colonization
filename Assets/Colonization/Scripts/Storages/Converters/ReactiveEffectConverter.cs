using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class ReactiveEffect
	{
        sealed public class Converter : JsonConverter
        {
            private readonly Type _type = typeof(ReactiveEffect);

            public override bool CanRead => true;
            public override bool CanWrite => true;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return ReadFromArray(serializer.Deserialize<int[]>(reader));
            }

            public static ReactiveEffect ReadFromArray(int[] array)
            {
                return new(new(array[0]), array[1], array[2], array[3], array[4], array[5]);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is not ReactiveEffect effect)
                    throw Errors.JsonSerialization(_type);

                WriteJsonArray(writer, effect);
            }

            public static void WriteJsonArray(JsonWriter writer, ReactiveEffect effect)
            {
                writer.WriteStartArray();
                writer.WriteValue(effect._code);
                writer.WriteValue(effect._targetAbility);
                writer.WriteValue(effect._typeModifier.Value);
                writer.WriteValue(effect._value);
                writer.WriteValue(effect._duration);
                writer.WriteValue(effect._skip);
                writer.WriteEndArray();
            }

            public override bool CanConvert(Type objectType) => _type.IsAssignableFrom(objectType);
        }
    }
}
