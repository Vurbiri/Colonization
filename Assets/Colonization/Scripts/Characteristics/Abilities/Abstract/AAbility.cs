//Assets\Colonization\Scripts\Characteristics\Abilities\Abstract\AAbility.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AAbility<TId> : IAbility, IValueId<TId>, IDisposable where TId : AbilityId<TId>
    {
        protected int _value;
        protected readonly Subscriber<int> _subscriber = new();

        public Id<TId> Id { get; }
        public int Value => _value;
        public bool IsValue => _value > 0;

        public AAbility(Id<TId> id, int value)
        {
            Id = id;
            _value = value;
        }

        public AAbility(AAbility<TId> other)
        {
            Id = other.Id;
            _value = other._value;
        }

        public abstract int AddModifier(IAbilityValue mod);
        public abstract int RemoveModifier(IAbilityValue mod);

        public Unsubscriber Subscribe(Action<int> action, bool calling = true) => _subscriber.Add(action, calling, _value);

        public void Dispose()
        {
            _subscriber.Dispose();
        }

        public static implicit operator int(AAbility<TId> ability) => ability._value;
        public static implicit operator bool(AAbility<TId> ability) => ability._value > 0;
    }
}
