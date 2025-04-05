//Assets\Vurbiri\Runtime\Reactive\Types\RBool.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Reactive
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    sealed public class RBool : ARType<bool>
    {
        public RBool() : base(false) { }
        public RBool(bool value) : base(value) { }

        public void True()
        {
            if (_value != true)
                _signer.Invoke(_value = true);
        }
        public void False()
        {
            if (_value != false)
                _signer.Invoke(_value = false);
        }
        public void Negation() => _signer.Invoke(_value = !_value);
        
        public static explicit operator RBool(bool value) => new(value);
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
    }
}
