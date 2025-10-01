using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization.Actors
{
	[Serializable, JsonConverter(typeof(HitSFXName.Converter))]
	public class HitSFXName : IEquatable<string>
	{
		[UnityEngine.SerializeField] private string _value;

        public string Value => _value;

        public HitSFXName() { }
        public HitSFXName(string value) => _value = value;

        public bool Equals(string str) => _value.Equals(str);

        public static implicit operator string(HitSFXName sfxName) => sfxName?._value;

        #region Nested Json Converter
        sealed public class Converter : AJsonConverter<HitSFXName>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new HitSFXName(serializer.Deserialize<string>(reader));
            }

            protected override void WriteJson(JsonWriter writer, HitSFXName value, JsonSerializer serializer)
            {
                writer.WriteValue(value._value);
            }
        }
        #endregion
    }
}
