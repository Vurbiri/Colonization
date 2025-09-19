using Newtonsoft.Json;
using UnityEngine;

namespace Vurbiri
{
    sealed internal class Vector2IntConverter : AJsonConverter<Vector2Int>
    {
        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = serializer.Deserialize<int[]>(reader);
            return new Vector2Int(data[0], data[1]);
        }

        protected override void WriteJson(JsonWriter writer, Vector2Int vector, JsonSerializer serializer)
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
