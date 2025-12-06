using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract class AAbility<TId> : Ability, IValueId<TId> where TId : AbilityId<TId>
    {
        protected int _maxValue = int.MaxValue;

        public Id<TId> Id { [Impl(256)] get; }
        public int MaxValue { [Impl(256)] get => _maxValue; [Impl(256)] set => SetMaxValue(value); }

        [Impl(256)] public AAbility(Id<TId> id, int value)
        {
            Id = id;
            _value = value;
        }

        [Impl(256)] public AAbility(AAbility<TId> other)
        {
            Id = other.Id;
            _value = other._value;
        }

        public virtual void SetMaxValue(int value)
        {
            int old = _value;
            _maxValue = value;
            _value = Math.Clamp(_value, 0, value);

            if (old != _value)
                _onChange.Invoke(_value);
        }

        public abstract int AddModifier(IAbilityValue mod);
        public abstract int RemoveModifier(IAbilityValue mod);
    }
}
