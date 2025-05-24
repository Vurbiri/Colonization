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
                var data = serializer.Deserialize<int[][]>(reader);

                return new GameState(data[0][0] > 0, data[0][1], data[1]);
            }

            protected override void WriteJson(JsonWriter writer, GameState state, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                    writer.WriteStartArray();
                        writer.WriteValue(state._isLoad ? 1 : 0);
                        writer.WriteValue(state._maxScore);
                    writer.WriteEndArray();
                    writer.WriteStartArray();
                        for(int i = 0; i < PlayerId.HumansCount; i++)
                            writer.WriteValue(state._score[i]);
                    writer.WriteEndArray();
                writer.WriteEndArray();
            }
        }
    }
}
