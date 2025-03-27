//Assets\Colonization\Scripts\Storages\Converters\ProfileConverter.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    public partial class Profile
	{
        sealed public class Converter : AJsonConverter<Profile>
        {
            private const int SIZE_ARRAY = 2;

            private readonly Profile _profile;

            public Converter() { }
            public Converter(Profile profile) => _profile = profile;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);
                Errors.ThrowIfLengthNotEqual(data.Length, SIZE_ARRAY);

                int i = 0;
                _profile._idLang = data[i++]; _profile._quality = data[i];
                _profile.Cancel();

                return _profile;
            }

            protected override void WriteJson(JsonWriter writer, Profile profile, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(profile._idLang);
                writer.WriteValue(profile._quality);
                writer.WriteEndArray();
            }
        }
    }
}
