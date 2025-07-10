using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri
{
    public class ColorConverter : AJsonConverter<Color>
	{
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = serializer.Deserialize<float[]>(reader);

            int i = 0;
            return new Color(data[i++], data[i++], data[i++], data[i++]);
        }

        protected override void WriteJson(JsonWriter writer, Color color, JsonSerializer serializer)
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
