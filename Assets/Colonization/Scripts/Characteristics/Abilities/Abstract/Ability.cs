using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class Ability : IReactiveValue<int>
    {
        protected int _value;
        protected readonly Subscription<int> _eventChanged = new();

        public int Value => _value;
        public bool IsValue => _value > 0;

        public Unsubscription Subscribe(Action<int> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _value);

        public static implicit operator int(Ability ability) => ability._value;
        public static implicit operator bool(Ability ability) => ability._value > 0;
    }
}
