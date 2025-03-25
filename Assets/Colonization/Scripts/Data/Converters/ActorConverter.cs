//Assets\Colonization\Scripts\Data\Converters\ActorConverter.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    [JsonConverter(typeof(Converter))]
    public abstract partial class Actor
    {
        sealed public class Converter : JsonConverter<Actor>
        {
            public override bool CanRead => false;

            public override Actor ReadJson(JsonReader reader, Type objectType, Actor existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                throw new NotSupportedException("Not supported deserialize type {Actor}");
            }

            public override void WriteJson(JsonWriter writer, Actor actor, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                Key.Converter.WriteJsonArray(writer, actor._currentHex.Key);
                StateWriteJson(writer, actor);
                for (int i = actor._effects.Count - 1; i >= 0; i--)
                    ReactiveEffect.Converter.WriteJsonArray(writer, actor._effects[i]);
                writer.WriteEndArray();
            }

            public static void StateWriteJson(JsonWriter writer, Actor actor)
            {
                writer.WriteStartArray();
                writer.WriteValue(actor._id);
                writer.WriteValue(actor._currentHP.Value);
                writer.WriteValue(actor._currentAP.Value);
                writer.WriteValue(actor._move.Value);
                writer.WriteEndArray();
            }
        }
    }
}
