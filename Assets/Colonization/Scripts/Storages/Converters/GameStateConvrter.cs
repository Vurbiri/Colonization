using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class GameState
    {
        sealed public class Converter : AJsonConverter<GameState>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);

                int i = 0;
                return new GameState(data[i++] > 0, data[i]);
            }

            protected override void WriteJson(JsonWriter writer, GameState state, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(state._isLoad ? 1 : 0);
                writer.WriteValue(state._maxScore);
                writer.WriteEndArray();
            }
        }
    }
}
