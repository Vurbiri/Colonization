using Newtonsoft.Json;
using System;

namespace Vurbiri.Reactive
{
    [Serializable, JsonConverter(typeof(Converter))]
    sealed public class RBool : ARType<bool>
    {
        public RBool() : base(false) { }
        public RBool(bool value) : base(value) { }

        public void True()
        {
            if (_value != true)
                _subscriber.Invoke(_value = true);
        }
        public void False()
        {
            if (_value != false)
                _subscriber.Invoke(_value = false);
        }
        public void Negation() => _subscriber.Invoke(_value = !_value);
        
        public static implicit operator bool(RBool value) => value._value;

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
