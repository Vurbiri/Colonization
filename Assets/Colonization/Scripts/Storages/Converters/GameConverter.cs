using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class GameLoop
    {
        sealed public class Converter : AJsonConverter<GameLoop>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);

                int i = 0;
                return new GameLoop(data[i++], new(data[i++], data[i++]), data[i++]);
            }

            protected override void WriteJson(JsonWriter writer, GameLoop game, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                {
                    writer.WriteValue(game._gameMode.Value);

                    writer.WriteValue(game._turnQueue.currentId.Value);
                    writer.WriteValue(game._turnQueue.round);

                    writer.WriteValue(game._hexId);
                }
                writer.WriteEndArray();
            }
        }
    }
}
