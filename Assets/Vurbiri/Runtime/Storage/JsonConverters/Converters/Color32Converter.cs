using Newtonsoft.Json;
using UnityEngine;

namespace Vurbiri
{
    sealed public class Color32Converter : AJsonConverter<Color32>
    {
        private Color32 _output;

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = serializer.Deserialize<string[]>(reader);
            int count = data.Length;
            if (count == 4)
            {
                _output = new(byte.Parse(data[0]), byte.Parse(data[1]), byte.Parse(data[2]), byte.Parse(data[3]));
            }
            else
            {
                var value = byte.Parse(data[0]);
                if(count == 1)
                    _output = new(value, value, value, value);
                else
                    _output = new(value, value, value, byte.Parse(data[1]));
            }
            return _output;
        }

        protected override void WriteJson(JsonWriter writer, Color32 color, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            {
                writer.WriteValue(color.r);

                if (color.r != color.g || color.r != color.b)
                {
                    writer.WriteValue(color.g);
                    writer.WriteValue(color.b);
                    writer.WriteValue(color.a);
                }
                else if (color.r != color.a)
                {
                    writer.WriteValue(color.a);
                }
            }
            writer.WriteEndArray();
        }
    }
}
