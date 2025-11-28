using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    sealed public class DependentAbility<TId> : AAbility<TId> where TId : AbilityId<TId>
    {
        private readonly ReactiveCombination<int, int> _combination;

        public DependentAbility(AAbility<TId> other, ReactiveValue<int> remove, ReactiveValue<int> add) : base(other)
        {
            _maxValue = _value;
            _combination = new(remove, add, Change);
        }

        public override int AddModifier(IAbilityValue mod) => 0;
        public override int RemoveModifier(IAbilityValue mod) => 0;

        private void Change(int remove, int add)
        {
            _changeEvent.Invoke(_value = _maxValue - remove + add);
        }
    }
}
