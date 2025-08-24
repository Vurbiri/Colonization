using System;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AbilityModifier
    {
        protected int _value = 0;

        public int Value => _value;

        protected AbilityModifier(int value) => _value = value;

        public abstract int Apply(int value);

        public void Add(int value) => _value += value;
    }

    sealed public class AbilityModifierAdd : AbilityModifier
    {
        public AbilityModifierAdd() : base(0) { }
        public AbilityModifierAdd(int value) : base(value) { }

        public override int Apply(int value) => Math.Max(value + _value, 0);
    }
        
    sealed public class AbilityModifierPercent : AbilityModifier
    {
        public AbilityModifierPercent() : base(100) { }
        public AbilityModifierPercent(int value) : base(value) { }

        public override int Apply(int value) => (int)Math.Round(Math.Max(value * _value, 0) / 100.0);
    }
}
