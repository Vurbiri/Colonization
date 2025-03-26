//Assets\Colonization\Scripts\Data\Converters\Road\RoadConverter.cs
using Newtonsoft.Json;
using Vurbiri.Colonization.Data;

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
                writer.WriteStartArray();
                foreach (var crossroad in value._crossroads)
                    Key.Converter.WriteJsonArray(writer, crossroad.Key);
                writer.WriteEndArray();
            }
        }
    }
}
