//Assets\Colonization\Scripts\Data\Converters\TurnQueueConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class TurnQueue
	{
        private const int SIZE_ARRAY = 3;

        public class Converter : JsonConverter<TurnQueue>
        {
            public override bool CanRead => true;
            public override bool CanWrite => true;

            public override TurnQueue ReadJson(JsonReader reader, Type objectType, TurnQueue existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                int[] data = serializer.Deserialize<int[]>(reader);
                Errors.ThrowIfLengthNotEqual(data, SIZE_ARRAY);
                int i = 0;
                return new(data[i++], data[i++], data[i]);
            }

            public override void WriteJson(JsonWriter writer, TurnQueue queue, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(queue._previousId.Value);
                writer.WriteValue(queue._currentId.Value);
                writer.WriteValue(queue._turn);
                writer.WriteEndArray();
            }
        }
    }
}
