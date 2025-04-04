//Assets\Colonization\Scripts\Characteristics\Abilities\Abstract\AAbilityChange.cs
using System;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AAbilitySettable<TId> : AAbility<TId> where TId : AbilityId<TId>
    {
        protected int _maxValue;

        public AAbilitySettable(AAbility<TId> other) :base(other) { }

        public int Set(int value)
        {
            value = Math.Clamp(value, 0, _maxValue);
            int delta = value - _value;
            _value = value;

            if (delta != 0)  _signer.Invoke(_value);

            return delta;
        }
    }
}
