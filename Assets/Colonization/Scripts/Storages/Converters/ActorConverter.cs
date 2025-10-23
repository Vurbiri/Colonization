using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public abstract partial class Actor
    {
        sealed public class Converter : AJsonConverter<Actor>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                int[][] data = serializer.Deserialize<int[][]>(reader);

                int n = 0;
                Key keyHex = new(data[n++]);
                ActorState state = StateReadFromArray(data[n++]);

                int count = data.Length - n;
                var effects = new ReactiveEffect[count];
                for (int l = 0; l < count; l++, n++)
                    effects[l] = ReactiveEffect.Converter.ReadFromArray(data[n]);

                return new ActorLoadData(keyHex, state, effects);
            }

            protected override void WriteJson(JsonWriter writer, Actor actor, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                Key.Converter.WriteToArray(writer, actor._currentHex.Key);
                StateWriteJson(writer, actor);
                for (int i = actor._effects.Count - 1; i >= 0; i--)
                    ReactiveEffect.Converter.WriteJsonArray(writer, actor._effects[i]);
                writer.WriteEndArray();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void StateWriteJson(JsonWriter writer, Actor actor)
            {
                writer.WriteStartArray();
                writer.WriteValue(actor._id);
                writer.WriteValue(actor._currentHP.Value);
                writer.WriteValue(actor._currentAP.Value);
                writer.WriteValue(actor._move.Value);
                writer.WriteValue(actor._zealCharge ? 1 : 0);
                writer.WriteEndArray();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static ActorState StateReadFromArray(int[] array)
            {
                int i = 0;
                return new(array[i++], array[i++], array[i++], array[i++], array[i++] > 0);
            }
        }
    }
}
