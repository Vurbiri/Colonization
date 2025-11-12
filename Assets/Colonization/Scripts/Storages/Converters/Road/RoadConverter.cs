using Newtonsoft.Json;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class Road
	{
        sealed public class Converter : AJsonConverter<Road>
        {
            public override bool CanRead => false;

            protected override void WriteJson(JsonWriter writer, Road value, JsonSerializer serializer)
            {
                WriteJsonArray(writer, value);
            }

            public static void WriteJsonArray(JsonWriter writer, Road value)
            {
                int count = value._links.Count;
                writer.WriteStartArray();
                for (int i = 0; i < count; ++i)
                    Key.Converter.WriteToArray(writer, value._links[i]);
                writer.WriteEndArray();
            }
        }
    }
}
