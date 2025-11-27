using System;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class SubAbility<TId> : ASettableAbility<TId> where TId : AbilityId<TId>
    {
        private readonly Ability _restore;
        private readonly IdArray<TypeModifierId, Func<int, int>> _modifiers = new();

        public int MaxValue { [Impl(256)] get => _maxValue; }
        public bool IsZero { [Impl(256)] get => _value == 0; }
        public bool IsMax { [Impl(256)] get => _value == _maxValue; }
        public bool IsNotMax { [Impl(256)] get => _value < _maxValue; }
        public int Percent { [Impl(256)] get => _value * 100 / _maxValue; }

        public SubAbility(AAbility<TId> self, Ability max, Ability restore) : base(self)
        {
            _modifiers[TypeModifierId.BasePercent] = OnBasePercent;
            _modifiers[TypeModifierId.Addition] = OnAddition;
            _modifiers[TypeModifierId.TotalPercent] = OnTotalPercent;

            _maxValue = max.Value;
            _value = _maxValue;

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
            int currentValue = Math.Clamp((int)((float)_value * newMaxValue / _maxValue + 0.5f), 0, newMaxValue);

            _maxValue = newMaxValue;

            if (currentValue != _value)
            {
                _value = currentValue;
                _changeEvent.Invoke(currentValue);
            }
        }
    }
}
