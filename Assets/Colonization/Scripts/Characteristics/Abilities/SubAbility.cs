using System;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class SubAbility<TId> : AAbilitySettable<TId> where TId : AbilityId<TId>
    {
        private readonly Ability _restore;
        private readonly IdArray<TypeModifierId, Func<int, int>> _modifiers = new();

        public bool IsMax => _value == _maxValue;
        public bool IsNotMax => _value < _maxValue;
        public int Percent => _value * 100 / _maxValue;

        public SubAbility(AAbility<TId> self, Ability max, Ability restore) : base(self)
        {
            _modifiers[TypeModifierId.BasePercent] = OnBasePercent;
            _modifiers[TypeModifierId.Addition] = OnAddition;
            _modifiers[TypeModifierId.TotalPercent] = OnTotalPercent;

            _value = _maxValue = max.Value;

            max.Subscribe(OnMaxChange, false);
            _restore = restore;
        }

        public override int AddModifier(IAbilityValue mod) => Set(_modifiers[mod.TypeModifier](mod.Value));
        public override int RemoveModifier(IAbilityValue mod) => Set(_modifiers[mod.TypeModifier](-mod.Value));

        public void Next()
        {
            if(_restore.Value <= 0)
                return;

            Set(_value + _restore.Value);
        }

        private int OnBasePercent(int value) => _value * (100 + value) / 100;
        private int OnAddition(int value) => _value + value;
        private int OnTotalPercent(int value) => _value + _maxValue * value / 100;

        private void OnMaxChange(int newMaxValue)
        {
            int oldMaxValue = _maxValue, currentValue = _value;
            _maxValue = newMaxValue;

            currentValue = (int)Math.Round((double)currentValue * newMaxValue / oldMaxValue);
            currentValue = Math.Clamp(currentValue, 0, newMaxValue);

            if(currentValue != _value)
            {
                _value = currentValue;
                _eventChanged.Invoke(currentValue);
            }
        }
    }
}
