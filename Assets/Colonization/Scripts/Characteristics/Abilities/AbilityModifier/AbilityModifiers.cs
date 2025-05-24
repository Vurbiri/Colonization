using System;

namespace Vurbiri.Colonization.Characteristics
{
    public class AbilityModifierAdd : IAbilityModifier
    {
        private int _value = 0;
        
        public Id<TypeModifierId> Id => TypeModifierId.Addition;

        public int Value { get => _value; set => _value = value; }

        public AbilityModifierAdd() { }
        public AbilityModifierAdd(int value) => _value = value;

        public int Apply(int value) => value + _value;
        
        public void Add(int value) => _value += value;
    }
        
    public class AbilityModifierPercent : IAbilityModifier
    {
        private int _value = 100;

        public Id<TypeModifierId> Id => TypeModifierId.BasePercent;
        public int Value { get => _value; set => _value = value; }

        public AbilityModifierPercent() { }
        public AbilityModifierPercent(int value) => _value = value;

        public int Apply(int value) => (int)Math.Round(Math.Max(value * _value, 0) / 100.0);

        public void Add(int value) => _value += value;
    }
}
