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
                return new HexLoadData(data[0], data[1]);
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
