//Assets\Colonization\Scripts\Data\Converters\ReactiveEffectConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization.Characteristics
{
    [JsonConverter(typeof(Converter))]
    public partial class ReactiveEffect
	{
        sealed public class Converter : JsonConverter
        {
            private const int SIZE_ARRAY = 5;

            public override bool CanRead => true;
            public override bool CanWrite => true;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return ReadFromArray(serializer.Deserialize<int[]>(reader));
            }

            public static ReactiveEffect ReadFromArray(int[] array)
            {
                Errors.ThrowIfLengthNotEqual(array, SIZE_ARRAY);
                int i = 0;
                return new(array[i++], array[i++], array[i++], array[i++], array[i]);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is not ReactiveEffect effect)
                    throw Errors.JsonSerialization(typeof(ReactiveEffect));

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
                writer.WriteEndArray();
            }

            public override bool CanConvert(Type objectType) => typeof(ReactiveEffect).IsAssignableFrom(objectType);
        }
    }
}
