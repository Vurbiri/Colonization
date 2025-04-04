//Assets\Vurbiri\Runtime\Utility\AudioMixer\AudioMixerConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri
{
    public partial class AudioMixer<T> where T : IdType<T>
    {
        sealed public class Converter : AJsonConverter<AudioMixer<T>>
        {
            private readonly AudioMixer<T> _mixer;
            
            public Converter() { }
            public Converter(AudioMixer<T> mixer) => _mixer = mixer;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var volumes = serializer.Deserialize<float[]>(reader);
                Throw.IfLengthNotEqual(volumes.Length, IdType<T>.Count);

                for (int i = 0; i < IdType<T>.Count; i++)
                {
                    _mixer[i] = volumes[i];
                    _mixer._volumes[i] = volumes[i];
                }

               return _mixer;
            }

            protected override void WriteJson(JsonWriter writer, AudioMixer<T> mixer, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                for (int i = 0; i < IdType<T>.Count; i++)
                    writer.WriteValue(mixer._volumes[i]);
                writer.WriteEndArray();
            }
        }
    }
}
