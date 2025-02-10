//Assets\Colonization\Scripts\Characteristics\Abilities\BooleanAbility.cs
using System;

namespace Vurbiri.Colonization.Characteristics
{
    public class BooleanAbility<TId> : AAbility<TId> where TId : AbilityId<TId>
    {
        public override int Value
        {
            get => _value;
            set => ChangeClamp(value);
        }

        public override bool IsValue
        {
            get => _value > 0;
            set => Change(value ? 1 : 0);
        }

        public BooleanAbility(AAbility<TId> other) : base(other)
        {
        }

        public void On() => Change(1);
        public void Off() => Change(0);

        public override int AddModifier(IAbilityModifierValue mod) => ChangeClamp(_value + mod.Value);
        public override int RemoveModifier(IAbilityModifierValue mod) => ChangeClamp(_value - mod.Value);

        private int Change(int value)
        {
            int delta = value - _value;
            _value = value;

            if (delta != 0)
                actionValueChange?.Invoke(_value);

            return delta;
        }

        private int ChangeClamp(int value)
        {
            value = Math.Clamp(value, 0, 1);
            int delta = value - _value;
            _value = value;

            if (delta != 0)
                actionValueChange?.Invoke(_value);

            return delta;
        }
    }
}
