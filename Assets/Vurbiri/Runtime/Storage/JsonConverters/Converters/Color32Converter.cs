using Newtonsoft.Json;
using UnityEngine;

namespace Vurbiri
{
    sealed public class Color32Converter : AJsonConverter<Color32>
    {
        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = serializer.Deserialize<byte[]>(reader);
            return new Color32(data[0], data[1], data[2], data[3]);
        }

        protected override void WriteJson(JsonWriter writer, Color32 color, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            {
                writer.WriteValue(color.r);
                writer.WriteValue(color.g);
                writer.WriteValue(color.b);
                writer.WriteValue(color.a);
            }
            writer.WriteEndArray();
        }
    }
}
