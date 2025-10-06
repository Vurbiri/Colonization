using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class Ability : ReactiveValue<int>
    {
        public bool IsValue { [Impl(256)] get => _value > 0; }

        [Impl(256)] sealed public override string ToString() => _value.ToString();

        public static implicit operator int(Ability ability) => ability._value;
        public static implicit operator bool(Ability ability) => ability._value > 0;
    }
}
