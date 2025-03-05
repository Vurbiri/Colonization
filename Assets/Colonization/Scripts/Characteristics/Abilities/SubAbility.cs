//Assets\Colonization\Scripts\Characteristics\Abilities\SubAbility.cs
using System;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public class SubAbility<TId> : AAbilityChange<TId> where TId : AbilityId<TId>
    {
        private readonly IAbility _restore;
        private readonly IdArray<TypeModifierId, Func<int, int>> _modifiers = new();

        public override int Value
        {
            get => _value;
            set => Change(value);
        }

        public SubAbility(AAbility<TId> self, IAbility max, IAbility restore) : base(self)
        {
            _modifiers[TypeModifierId.BasePercent] = OnBasePercent;
            _modifiers[TypeModifierId.Addition] = OnAddition;
            _modifiers[TypeModifierId.TotalPercent] = OnTotalPercent;

            _value = _maxValue = max.Value;

            max.Subscribe(OnMaxChange, false);
            _restore = restore;
        }

        public override int AddModifier(IAbilityModifierValue mod) => Change(_modifiers[mod.TypeModifier](mod.Value));
        public override int RemoveModifier(IAbilityModifierValue mod) => Change(_modifiers[mod.TypeModifier](-mod.Value));

        public void Next()
        {
            if(_restore.Value <= 0)
                return;

            Change(_value + _restore.Value);
        }

        private int OnBasePercent(int value) => _value * (100 + value) / 100;
        private int OnAddition(int value) => _value + value;
        private int OnTotalPercent(int value) => _value + _maxValue * value / 100;

        private void OnMaxChange(int value)
        {
            int delta = value - _maxValue;
            if (delta == 0)
                return;

            _maxValue = value;

            if (delta > 0)
                Change(_value + delta);
            else
                Change(_value);
        }
    }
}
