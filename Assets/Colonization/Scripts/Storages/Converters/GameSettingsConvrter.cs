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

                int i = 0;
                return new GameSettings(data[i++] > 0, data[i++], data[i++] > 0, data[i++] > 0);
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
