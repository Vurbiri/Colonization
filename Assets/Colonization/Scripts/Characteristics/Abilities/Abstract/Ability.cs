using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract class Ability : Reactive<int>
    {
        public bool IsTrue { [Impl(256)] get => _value > 0; }
        public bool IsFalse { [Impl(256)] get => _value == 0; }

        public static implicit operator int(Ability ability) => ability._value;
        public static implicit operator bool(Ability ability) => ability._value > 0;

        public static int operator -(Ability a, int i) => a._value - i;
        public static int operator -(int i, Ability a) => i - a._value;

        public static int operator +(Ability a, int i) => a._value + i;
        public static int operator +(int i, Ability a) => i + a._value;

        public static int operator *(Ability a, int i) => a._value * i;
        public static int operator *(int i, Ability a) => i * a._value;

        public static int operator /(Ability a, int i) => a._value / i;
        public static int operator /(int i, Ability a) => i / a._value;
    }
}
