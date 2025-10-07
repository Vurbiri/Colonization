using System;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ASettableAbility<TId> : AAbility<TId> where TId : AbilityId<TId>
    {
        protected int _maxValue;

        public ASettableAbility(AAbility<TId> other) :base(other) { }

        public int Set(int value)
        {
            value = Math.Clamp(value, 0, _maxValue);
            int delta = value - _value;
            _value = value;

            if (delta != 0)  _changeEvent.Invoke(_value);

            return delta;
        }
    }
}
