//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\AbilityModifiers.cs
namespace Vurbiri.Colonization.Characteristics
{
    public static class AbilityModifierFactory
    {
        public static IAbilityModifier Create(int type, int value) => type switch
        {
            TypeModifierId.Addition => new AbilityModifierAdd(value),
            TypeModifierId.Percent => new AbilityModifierPercent(value),
            _ => null,
        };
    }

    public class AbilityModifierAdd : IAbilityModifier
    {
        private const int DEFAULT_VALUE = 0;
        
        private int _value = DEFAULT_VALUE;
        
        public Id<TypeModifierId> Id => TypeModifierId.Addition;

        public int Value { get => _value; set => _value = value; }

        public AbilityModifierAdd() { }
        public AbilityModifierAdd(int value) => _value = value;

        public int Apply(int value) => value + _value;
        public int Apply(int value, int modifier) => value + modifier;
        
        public void Add(int value) => _value += value;

        public void Reset() => _value = DEFAULT_VALUE;
    }
        
    public class AbilityModifierPercent : IAbilityModifier
    {
        private const int DEFAULT_VALUE = 100;

        private int _value = DEFAULT_VALUE;

        public Id<TypeModifierId> Id => TypeModifierId.Percent;
        public int Value { get => _value; set => _value = value; }

        public AbilityModifierPercent() { }
        public AbilityModifierPercent(int value) => _value = value;

        public int Apply(int value) => UnityEngine.Mathf.RoundToInt(value * _value / 100f);
        public int Apply(int value, int modifier) => UnityEngine.Mathf.RoundToInt(value * modifier / 100f);

        public void Add(int value) => _value += value;

        public void Reset() => _value = DEFAULT_VALUE;
    }
}
