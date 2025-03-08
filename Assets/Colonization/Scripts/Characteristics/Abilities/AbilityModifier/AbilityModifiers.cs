//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\AbilityModifiers.cs
using System;

namespace Vurbiri.Colonization.Characteristics
{
    public static class AbilityModifierFactory
    {
        public static IAbilityModifier Create(Id<TypeModifierId> type, int value)
        {
            if (type == TypeModifierId.Addition)
                return new AbilityModifierAdd(value);

            return new AbilityModifierPercent(value);
        }
    }

    public class AbilityModifierAdd : IAbilityModifier
    {
        private int _value = 0;
        
        public Id<TypeModifierId> Id => TypeModifierId.Addition;

        public int Value { get => _value; set => _value = value; }

        public AbilityModifierAdd() { }
        public AbilityModifierAdd(int value) => _value = value;

        public int Apply(int value) => value + _value;
        public int Apply(int value, int modifier) => value + modifier;
        
        public void Add(int value) => _value += value;
    }
        
    public class AbilityModifierPercent : IAbilityModifier
    {
        private int _value = 100;

        public Id<TypeModifierId> Id => TypeModifierId.BasePercent;
        public int Value { get => _value; set => _value = value; }

        public AbilityModifierPercent() { }
        public AbilityModifierPercent(int value) => _value = value;

        public int Apply(int value) => (int)Math.Round(value * _value / 100.0);
        public int Apply(int value, int modifier) => (int)Math.Round(value * modifier / 100.0);

        public void Add(int value) => _value += value;
    }
}
