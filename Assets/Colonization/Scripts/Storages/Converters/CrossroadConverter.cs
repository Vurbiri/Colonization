using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    public partial class Crossroad
	{
		sealed public class Converter : AJsonConverter<Crossroad, EdificeLoadData>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);

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
