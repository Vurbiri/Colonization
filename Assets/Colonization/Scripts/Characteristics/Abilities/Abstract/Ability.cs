//Assets\Colonization\Scripts\Characteristics\Abilities\Abstract\Ability.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class Ability : IReactiveValue<int>
    {
        protected int _value;
        protected readonly Subscriber<int> _subscriber = new();

        public int Value => _value;
        public bool IsValue => _value > 0;

        public Unsubscriber Subscribe(Action<int> action, bool calling = true) => _subscriber.Add(action, calling, _value);

        public void Dispose()
        {
            _subscriber.Dispose();
        }

        public static implicit operator int(Ability ability) => ability._value;
        public static implicit operator bool(Ability ability) => ability._value > 0;
    }
}
