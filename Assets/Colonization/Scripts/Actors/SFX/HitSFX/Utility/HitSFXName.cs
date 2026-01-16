using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	[Serializable, JsonConverter(typeof(HitSFXName.Converter))]
	public class HitSFXName : IEquatable<string>
	{
		[UnityEngine.SerializeField] private string _value;

        public string Value { [Impl(256)] get => _value; }

        public HitSFXName() { }
        [Impl(256)] public HitSFXName(string value) => _value = value;

        [Impl(256)] public bool Equals(string str) => _value.Equals(str);

        [Impl(256)] public static implicit operator string(HitSFXName sfxName) => sfxName?._value;

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
