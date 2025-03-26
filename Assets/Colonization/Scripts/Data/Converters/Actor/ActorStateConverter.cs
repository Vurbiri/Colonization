//Assets\Colonization\Scripts\Data\Converters\Actor\ActorStateConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization.Data
{
    public class ActorStateConverter : JsonConverter<ActorState>
    {
        private const int SIZE_ARRAY = 4;

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override ActorState ReadJson(JsonReader reader, Type objectType, ActorState existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return ReadFromArray(serializer.Deserialize<int[]>(reader));
        }

        public static ActorState ReadFromArray(int[] array)
        {
            Errors.ThrowIfLengthNotEqual(array, SIZE_ARRAY);
            int i = 0;
            return new(array[i++], array[i++], array[i++], array[i]);
        }

        public override void WriteJson(JsonWriter writer, ActorState value, JsonSerializer serializer)
        {
            throw new NotSupportedException("Not supported serialize type {ActorState}");
        }
    }
}
