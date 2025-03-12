//Assets\Colonization\Scripts\Characteristics\Abilities\BooleanAbility.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class BooleanAbility<TId> : AAbilityChange<TId> where TId : AbilityId<TId>
    {
        public override int Value
        {
            get => _value;
            set => Change(value);
        }

        public override bool IsValue
        {
            get => _value > 0;
            set => ChangeNotClamp(value ? 1 : 0);
        }

        public BooleanAbility(AAbility<TId> other) : base(other)
        {
            _maxValue = 1;
        }

        public void On() => ChangeNotClamp(1);
        public void Off() => ChangeNotClamp(0);

        public override int AddModifier(IAbilityValue mod) => Change(_value + mod.Value);
        public override int RemoveModifier(IAbilityValue mod) => Change(_value - mod.Value);

        private int ChangeNotClamp(int value)
        {
            int delta = value - _value;
            _value = value;

            if (delta != 0)
                _subscriber.Invoke(_value);

            return delta;
        }
    }
}
