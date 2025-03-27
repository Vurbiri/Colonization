//Assets\Colonization\Scripts\Storages\Converters\SatanConverter.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public partial class Satan
	{
		sealed public class Converter : AJsonConverter<Satan>
        {
            private const int SIZE_ARRAY = 5;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);
                Errors.ThrowIfLengthNotEqual(data.Length, SIZE_ARRAY);

                int i = 0;
                return new SatanLoadState(data[i++], data[i++], data[i++], data[i++], data[i]);
            }

            protected override void WriteJson(JsonWriter writer, Satan satan, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(satan._level);
                writer.WriteValue(satan._curse);
                writer.WriteValue(satan._balance);
                writer.WriteValue(satan._spawner.Potential);
                writer.WriteValue(satan._demons.Capacity);
                writer.WriteEndArray();
            }
        }
	}
}
