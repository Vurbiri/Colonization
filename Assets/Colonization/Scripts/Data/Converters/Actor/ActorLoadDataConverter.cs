//Assets\Colonization\Scripts\Data\Converters\Actor\ActorLoadDataConverter.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Data
{
    public class ActorLoadDataConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int[][] data = serializer.Deserialize<int[][]>(reader);

            int n = 0;
            Key keyHex = new(data[n++]);
            ActorState state = ActorStateConverter.ReadFromArray(data[n++]);

            int count = data.Length - n;
            var effects = new ReactiveEffect[count];
            for (int l = 0; l < count; l++, n++)
                effects[l] = ReactiveEffect.Converter.ReadFromArray(data[n]);

            return new ActorLoadData(keyHex, state, effects);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException("Not supported serialize type {ActorDataConverter}");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Roads) == objectType;
        }
    }
}
