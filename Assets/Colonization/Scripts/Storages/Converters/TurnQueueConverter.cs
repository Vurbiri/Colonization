//Assets\Colonization\Scripts\Storages\Converters\TurnQueueConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class TurnQueue
	{
        sealed public class Converter : AJsonConverter<TurnQueue>
        {
            private const int SIZE_ARRAY = 3;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                int[] data = serializer.Deserialize<int[]>(reader);
                Throw.IfLengthNotEqual(data.Length, SIZE_ARRAY);

                int i = 0;
                return new TurnQueue(data[i++], data[i++], data[i]);
            }

            protected override void WriteJson(JsonWriter writer, TurnQueue queue, JsonSerializer serializer)
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
