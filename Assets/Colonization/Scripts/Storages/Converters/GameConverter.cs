using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    public partial class GameLoop
    {
        sealed public class Converter : AJsonConverter<GameLoop>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);
                return new GameLoop(data[0], new(data[1], data[2]), data[3]);
            }

            protected override void WriteJson(JsonWriter writer, GameLoop game, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                {
                    writer.WriteValue(game._gameMode.Value);

                    writer.WriteValue(game._turnQueue.currentId.Value);
                    writer.WriteValue(game._turnQueue.turn);

                    writer.WriteValue(game._hexId);
                }
                writer.WriteEndArray();
            }
        }
    }
}
