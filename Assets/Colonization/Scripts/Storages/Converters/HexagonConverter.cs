//Assets\Colonization\Scripts\Storages\Converters\HexagonConverter.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class Hexagon
	{
        sealed public class Converter : AJsonConverter<Hexagon>
        {

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                int[] data = serializer.Deserialize<int[]>(reader);

                int i = 0;
                return new HexLoadData(data[i++], data[i]);
            }

            protected override void WriteJson(JsonWriter writer, Hexagon hex, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(hex._id);
                writer.WriteValue(hex._surfaceId);
                writer.WriteEndArray();
            }
        }
    }
}
