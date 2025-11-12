using Newtonsoft.Json;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class Score
    {
        sealed public class Converter : AJsonConverter<Score>
        {
            public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new Score(serializer.Deserialize<int[]>(reader));
            }

            protected override void WriteJson(JsonWriter writer, Score score, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                for (int i = 0; i < PlayerId.HumansCount; ++i)
                    writer.WriteValue(score._values[i]);
                writer.WriteEndArray();
            }
        }
    }
}
