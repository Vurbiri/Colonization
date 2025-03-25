//Assets\Colonization\Scripts\Data\Converters\ActorConverter.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    [JsonConverter(typeof(Converter))]
    public abstract partial class Actor
    {
        sealed public class Converter : JsonConverter<Actor>
        {
            public override bool CanRead => false;

            public override Actor ReadJson(JsonReader reader, Type objectType, Actor existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                throw new NotSupportedException("Not supported deserialize type {Actor}");
            }

            public override void WriteJson(JsonWriter writer, Actor actor, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                Key.Converter.WriteJsonArray(writer, actor._currentHex.Key);
                StateWriteJson(writer, actor);
                for (int i = actor._effects.Count - 1; i >= 0; i--)
                    ReactiveEffect.Converter.WriteJsonArray(writer, actor._effects[i]);
                writer.WriteEndArray();
            }

            public static void StateWriteJson(JsonWriter writer, Actor actor)
            {
                writer.WriteStartArray();
                writer.WriteValue(actor._id);
                writer.WriteValue(actor._currentHP.Value);
                writer.WriteValue(actor._currentAP.Value);
                writer.WriteValue(actor._move.Value);
                writer.WriteEndArray();
            }
        }

        private const int ADD_SIZE_ARRAY = 2, SIZE_DATA_ARRAY = 4;
        public int[][] ToArray()
        {
            int i = 0;
            int count = _effects.Count;
            int[][] array = new int[count + ADD_SIZE_ARRAY][];

            array[i++] = _currentHex.Key.ToArray();
            array[i++] = ToDataArray(null);

            for (int j = 0; j < count; j++, i++)
                array[i] = _effects[j].ToArray();

            return array;
        }
        public int[][] ToArray(int[][] array)
        {
            int count = _effects.Count;
            if (array == null || array.Length != count + ADD_SIZE_ARRAY)
                return ToArray();

            int i = 0;
            array[i] = _currentHex.Key.ToArray(array[i++]);
            array[i] = ToDataArray(array[i++]);

            for (int j = 0; j < count; j++, i++)
                array[i] = _effects[j].ToArray(array[i]);

            return array;
        }

        private int[] ToDataArray(int[] array)
        {
            if (array == null || array.Length != SIZE_DATA_ARRAY)
                array = new int[SIZE_DATA_ARRAY];

            int i = 0;
            array[i++] = _id; array[i++] = _currentHP.Value; array[i++] = _currentAP.Value; array[i] = _move.Value;

            return array;
        }

    }
}
