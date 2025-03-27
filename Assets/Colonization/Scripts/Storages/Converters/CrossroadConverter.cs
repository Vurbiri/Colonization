//Assets\Colonization\Scripts\Storages\Converters\CrossroadConverter.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class Crossroad
	{
		public class Converter : AJsonConverter<Crossroad>
        {
            private const int SIZE_ARRAY = 4;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);
                Errors.ThrowIfLengthNotEqual(data.Length, SIZE_ARRAY);

                int i = 0;
                return new EdificeLoadData(new(data[i++], data[i++]), data[i++], data[i] > 0);
            }

            protected override void WriteJson(JsonWriter writer, Crossroad cross, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(cross._key.X);
                writer.WriteValue(cross._key.Y);
                writer.WriteValue(cross._states.id.Value);
                writer.WriteValue(cross._isWall ? 1 : 0);
                writer.WriteEndArray();
            }
        }
    }
}
