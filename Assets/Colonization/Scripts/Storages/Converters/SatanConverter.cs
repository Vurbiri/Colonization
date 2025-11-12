using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class Satan
	{
		sealed public class Converter : AJsonConverter<Satan, SatanLoadState>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);
                return new SatanLoadState(data[0], data[1], data[2]);
            }

            protected override void WriteJson(JsonWriter writer, Satan satan, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(satan._leveling.Level);
                writer.WriteValue(satan._curse);
                writer.WriteValue(satan._spawner.Potential);
                writer.WriteEndArray();
            }
        }
	}
}
