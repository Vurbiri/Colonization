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
                return new EdificeLoadData(new(data[0], data[1]), data[2], data[3] > 0);
            }

            protected override void WriteJson(JsonWriter writer, Crossroad cross, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(cross._key.x);
                writer.WriteValue(cross._key.y);
                writer.WriteValue(cross._states.id.Value);
                writer.WriteValue(cross._isWall ? 1 : 0);
                writer.WriteEndArray();
            }
        }
    }
}
