using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class GameSettings
    {
        sealed public class Converter : AJsonConverter<GameSettings>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);
                return new GameSettings(data[0] > 0, data[1], data[2] > 0, data[3] > 0);
            }

            protected override void WriteJson(JsonWriter writer, GameSettings settings, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(settings._isLoad ? 1 : 0);
                writer.WriteValue(settings._maxScore);
                writer.WriteValue(settings._hexagonShow ? 1 : 0);
                writer.WriteValue(settings._trackingCamera ? 1 : 0);
                writer.WriteEndArray();
            }
        }
    }
}
