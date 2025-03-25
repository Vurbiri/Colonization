//Assets\Colonization\Scripts\Data\PlayersData\PlayerLoadData\ActorState.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization.Data
{
    public readonly struct ActorState
	{
        public readonly int id;
        public readonly int currentHP;
        public readonly int currentAP;
        public readonly int move;

        public ActorState(int id, int currentHP, int currentAP, int move)
        {
            this.id = id;
            this.currentHP = currentHP;
            this.currentAP = currentAP;
            this.move = move;
        }

        #region Nested: Converter
        //***********************************
        sealed public class Converter : JsonConverter<ActorState>
        {
            private const int SIZE_ARRAY = 4;

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
        #endregion
    }
}
