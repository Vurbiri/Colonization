//Assets\Colonization\Scripts\Storages\Converters\GameDataConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    public partial class GameSettings
    {
        sealed public class Converter : AJsonConverter<GameData>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);

                int i = 0;
                return new GameData(data[i++] > 0, data[i]);
            }

            protected override void WriteJson(JsonWriter writer, GameData data, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(data.newGame ? 1 : 0);
                writer.WriteValue(data.maxScore);
                writer.WriteEndArray();
            }
        }
    }
}
