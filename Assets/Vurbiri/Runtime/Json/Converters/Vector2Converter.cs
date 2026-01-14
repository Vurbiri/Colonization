using Newtonsoft.Json;
using UnityEngine;

namespace Vurbiri
{
    sealed internal class Vector2Converter : AJsonConverter<Vector2>
    {
        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = serializer.Deserialize<float[]>(reader);
            return new Vector2(data[0], data[1]);
        }

        protected override void WriteJson(JsonWriter writer, Vector2 vector, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            {
                writer.WriteValue(vector.x);
                writer.WriteValue(vector.y);
            }
            writer.WriteEndArray();
        }
    }
}
