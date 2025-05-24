using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Profile
	{
        sealed public class Converter : AJsonConverter<Profile>
        {
            private readonly Profile _profile;

            public Converter() { }
            public Converter(Profile profile) => _profile = profile;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);

                int i = 0;
                _profile._idLang = (SystemLanguage)data[i++]; _profile._quality = data[i];
                _profile.Cancel();

                return _profile;
            }

            protected override void WriteJson(JsonWriter writer, Profile profile, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue((int)profile._idLang);
                writer.WriteValue(profile._quality);
                writer.WriteEndArray();
            }
        }
    }
}
