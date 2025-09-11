using Newtonsoft.Json;
using UnityEngine;

namespace Vurbiri
{
    sealed internal class Vector3IntConverter : AJsonConverter<Vector3Int>
    {
        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = serializer.Deserialize<int[]>(reader);
            return new Vector3Int(data[0], data[1], data[2]);
        }

        protected override void WriteJson(JsonWriter writer, Vector3Int vector, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            {
                writer.WriteValue(vector.x);
                writer.WriteValue(vector.y);
                writer.WriteValue(vector.z);
            }
            writer.WriteEndArray();
        }
    }
}
