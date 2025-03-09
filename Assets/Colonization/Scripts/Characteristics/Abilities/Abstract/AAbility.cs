//Assets\Colonization\Scripts\Characteristics\Abilities\Abstract\AAbility.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AAbility<TId> : IAbility, IValueId<TId> where TId : AbilityId<TId>
    {
        protected int _value;
        protected Subscriber<int> _subscriber = new();

        public Id<TId> Id { get; }
        public virtual int Value { get => _value; set { } }
        public virtual bool IsValue { get => _value > 0; set { } }

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

        public abstract int AddModifier(IAbilityModifierValue mod);
        public abstract int RemoveModifier(IAbilityModifierValue mod);

        public Unsubscriber Subscribe(Action<int> action, bool calling = true)
        {
            if (calling) action(_value);
            return _subscriber.Add(action);
        }
    }
}
