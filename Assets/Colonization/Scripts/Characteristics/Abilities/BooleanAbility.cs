//Assets\Colonization\Scripts\Characteristics\Abilities\BooleanAbility.cs
namespace Vurbiri.Colonization.Characteristics
{
    sealed public class BooleanAbility<TId> : AAbilitySettable<TId> where TId : AbilityId<TId>
    {
        public BooleanAbility(AAbility<TId> other) : base(other)
        {
            _maxValue = 1;
        }

        public void On() => SetNotClamp(1);
        public void Off() => SetNotClamp(0);

        public override int AddModifier(IAbilityValue mod) => Set(_value + mod.Value);
        public override int RemoveModifier(IAbilityValue mod) => Set(_value - mod.Value);

        private int SetNotClamp(int value)
        {
            int delta = value - _value;
            _value = value;

            if (delta != 0)
                _signer.Invoke(_value);

            return delta;
        }
    }
}
