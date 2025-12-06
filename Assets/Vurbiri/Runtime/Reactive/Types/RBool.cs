using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
    [Serializable, JsonConverter(typeof(Converter))]
    sealed public class RBool : AReactiveType<bool>
    {
        [Impl(256)] public RBool() : base(false) { }
        [Impl(256)] public RBool(bool value) : base(value) { }

        [Impl(256)] public void True()
        {
            if (!_value) _onChange.Invoke(_value = true);
        }
        [Impl(256)] public void False()
        {
            if (_value)  _onChange.Invoke(_value = false);
        }
        [Impl(256)] public void Negation() => _onChange.Invoke(_value = !_value);

        [Impl(256)] public static implicit operator bool(RBool value) => value._value;

        #region Logic operator
        public static bool operator !(RBool r) => !r._value;

        public static bool operator |(RBool a, RBool b) => a._value | b._value;
        public static bool operator |(RBool r, IReactiveValue<bool> i) => r._value | i.Value;
        public static bool operator |(IReactiveValue<bool> i, RBool r) => i.Value | r._value;
        public static bool operator |(RBool r, bool i) => r._value | i;
        public static bool operator |(bool i, RBool r) => i | r._value;

        public static bool operator &(RBool a, RBool b) => a._value & b._value;
        public static bool operator &(RBool r, IReactiveValue<bool> i) => r._value & i.Value;
        public static bool operator &(IReactiveValue<bool> i, RBool r) => i.Value & r._value;
        public static bool operator &(RBool r, bool i) => r._value & i;
        public static bool operator &(bool i, RBool r) => i & r._value;

        public static bool operator ^(RBool a, RBool b) => a._value ^ b._value;
        public static bool operator ^(RBool r, IReactiveValue<bool> i) => r._value ^ i.Value;
        public static bool operator ^(IReactiveValue<bool> i, RBool r) => i.Value ^ r._value;
        public static bool operator ^(RBool r, bool i) => r._value ^ i;
        public static bool operator ^(bool i, RBool r) => i ^ r._value;
        #endregion

        #region Nested JsonConverter
        //***************************************************
        sealed public class Converter : AJsonConverter<RBool>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new RBool(serializer.Deserialize<bool>(reader));
            }

            protected override void WriteJson(JsonWriter writer, RBool value, JsonSerializer serializer)
            {
                writer.WriteValue(value._value);
            }
        }
        #endregion
        }
}
