//Assets\Colonization\Scripts\Characteristics\Abilities\Abstract\AAbility.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AAbility<TId> : IReadOnlyReactive<int>, IValueId<TId> where TId : AbilityId<TId>
    {
        private readonly Id<TId> _id;

        protected int _value;

        protected Action<int> actionValueChange;

        public Id<TId> Id => _id;
        public virtual int Value { get => _value; set { } }
        public virtual bool IsValue { get => _value > 0; set { } }

        public AAbility(Id<TId> id, int value)
        {
            _id = id;
            _value = value;
        }

        public AAbility(AAbility<TId> other)
        {
            _id = other._id;
            _value = other._value;
        }

        public abstract int AddModifier(IAbilityModifierValue mod);
        public abstract int RemoveModifier(IAbilityModifierValue mod);

        public IUnsubscriber Subscribe(Action<int> action, bool calling = true)
        {
            actionValueChange += action;
            if (calling)
                action(_value);

            return new Unsubscriber<Action<int>>(this, action);
        }

        public void Unsubscribe(Action<int> action) => actionValueChange -= action;
    }
}
