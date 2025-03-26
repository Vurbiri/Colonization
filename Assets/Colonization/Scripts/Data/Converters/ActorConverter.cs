//Assets\Colonization\Scripts\Data\Converters\ActorConverter.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    [JsonConverter(typeof(Converter))]
    public abstract partial class Actor
    {
        sealed public class Converter : AJsonConverter<Actor>
        {
            private const int STATE_SIZE_ARRAY = 4;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                //UnityEngine.Debug.LogWarning("Actor - ReadJson");
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
                Key.Converter.WriteJsonArray(writer, actor._currentHex.Key);
                StateWriteJson(writer, actor);
                for (int i = actor._effects.Count - 1; i >= 0; i--)
                    ReactiveEffect.Converter.WriteJsonArray(writer, actor._effects[i]);
                writer.WriteEndArray();
            }

            private void StateWriteJson(JsonWriter writer, Actor actor)
            {
                writer.WriteStartArray();
                writer.WriteValue(actor._id);
                writer.WriteValue(actor._currentHP.Value);
                writer.WriteValue(actor._currentAP.Value);
                writer.WriteValue(actor._move.Value);
                writer.WriteEndArray();
            }

            private ActorState StateReadFromArray(int[] array)
            {
                Errors.ThrowIfLengthNotEqual(array, STATE_SIZE_ARRAY);
                int i = 0;
                return new(array[i++], array[i++], array[i++], array[i]);
            }
        }
    }
}
