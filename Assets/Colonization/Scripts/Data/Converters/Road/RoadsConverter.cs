//Assets\Colonization\Scripts\Data\Converters\Road\RoadsConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class Roads
    {
        sealed public class Converter : JsonConverter
        {
            public override bool CanRead => false;
            public override bool CanWrite => true;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotSupportedException("Not supported deserialize type {Roads}");
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                //if (writer is JsonTextWriter jsonTextWriter)
                //{
                //jsonTextWriter.QuoteChar = '\'';
                //jsonTextWriter.QuoteName = false;
                //jsonTextWriter.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                //}

                if (value is not Roads roads) return;

                int count = roads._roadsLists.Count;
                writer.WriteStartArray();
                for (int i = 0; i < count; i++)
                    Road.Converter.WriteJsonArray(writer, roads._roadsLists[i]);
                writer.WriteEndArray();
            }

            public override bool CanConvert(Type objectType) => typeof(Roads).IsAssignableFrom(objectType);
        }
    }
}
