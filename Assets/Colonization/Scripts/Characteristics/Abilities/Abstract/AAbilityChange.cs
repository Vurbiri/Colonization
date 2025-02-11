//Assets\Colonization\Scripts\Characteristics\Abilities\Abstract\AAbilityChange.cs
using System;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AAbilityChange<TId> : AAbility<TId> where TId : AbilityId<TId>
    {
        protected int _maxValue;

        public AAbilityChange(AAbility<TId> other) :base(other) { }

        protected int Change(int value)
        {
            value = Math.Clamp(value, 0, _maxValue);
            int delta = value - _value;

            _value = value;

            if (delta != 0)
                actionValueChange?.Invoke(_value);

            return delta;
        }
    }
}
