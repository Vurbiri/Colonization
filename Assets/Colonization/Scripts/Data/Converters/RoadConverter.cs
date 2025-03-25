//Assets\Colonization\Scripts\Data\Converters\RoadConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Road.Converter))]
    public partial class Road
	{
        sealed public class Converter : JsonConverter<Road>
        {
            public override bool CanRead => false;

            public override Road ReadJson(JsonReader reader, Type objectType, Road existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                throw new NotSupportedException("Not supported deserialize type {Road}");
            }

            public override void WriteJson(JsonWriter writer, Road value, JsonSerializer serializer)
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
