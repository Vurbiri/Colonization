using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    [Serializable, JsonConverter(typeof(Converter))]
    sealed public class RefBool : RefValue<bool>
    {
        [Impl(256)] public RefBool() { }
        [Impl(256)] public RefBool(bool value) => _value = value;

        [Impl(256)] public void True() => _value = true;
        [Impl(256)] public void False() => _value = false;
        [Impl(256)] public void Negation() => _value = !_value;

        [Impl(256)] public static implicit operator bool(RefBool value) => value._value;

        #region Logic operator
        [Impl(256)] public static bool operator !(RefBool r) => !r._value;

        [Impl(256)] public static bool operator |(RefBool a, RefBool b) => a._value | b._value;
        [Impl(256)] public static bool operator |(RefBool r, bool i) => r._value | i;
        [Impl(256)] public static bool operator |(bool i, RefBool r) => i | r._value;

        [Impl(256)] public static bool operator &(RefBool a, RefBool b) => a._value & b._value;
        [Impl(256)] public static bool operator &(RefBool r, bool i) => r._value & i;
        [Impl(256)] public static bool operator &(bool i, RefBool r) => i & r._value;

        [Impl(256)] public static bool operator ^(RefBool a, RefBool b) => a._value ^ b._value;
        [Impl(256)] public static bool operator ^(RefBool r, bool i) => r._value ^ i;
        [Impl(256)] public static bool operator ^(bool i, RefBool r) => i ^ r._value;
        #endregion

        #region Nested JsonConverter
        //***************************************************
        sealed public class Converter : AJsonConverter<RefBool>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new RefBool(serializer.Deserialize<bool>(reader));
            }

            protected override void WriteJson(JsonWriter writer, RefBool value, JsonSerializer serializer)
            {
                writer.WriteValue(value._value);
            }
        }
        #endregion
    }
}
