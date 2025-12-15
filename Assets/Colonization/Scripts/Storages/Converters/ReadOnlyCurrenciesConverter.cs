using Newtonsoft.Json;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public abstract partial class ReadOnlyCurrencies
    {
        sealed public class Converter : AJsonConverter<ReadOnlyCurrencies>
        {
            public override bool CanRead => false;

            protected override void WriteJson(JsonWriter writer, ReadOnlyCurrencies currencies, JsonSerializer serializer)
            {
                var values = currencies._values;

                writer.WriteStartArray();
                for(int i = 0; i < CurrencyId.Count; ++i)
                    writer.WriteValue(values[i].Value);
                writer.WriteValue(currencies._blood.Value);
                writer.WriteEndArray();
            }
        }
    }
}
