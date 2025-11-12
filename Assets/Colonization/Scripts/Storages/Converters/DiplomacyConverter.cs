using Newtonsoft.Json;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class Diplomacy
    {
        sealed public class Converter : AJsonConverter<Diplomacy>
        {
            public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new Diplomacy(serializer.Deserialize<int[]>(reader));
            }

            protected override void WriteJson(JsonWriter writer, Diplomacy diplomacy, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                for (int i = 0; i < PlayerId.HumansCount; ++i)
                    writer.WriteValue(diplomacy._values[i]);
                writer.WriteEndArray();
            }
        }
    }
}
