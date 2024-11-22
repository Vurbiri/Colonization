//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\AbilityModifiers.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class AbilityModAdd : IAbilityModifier
    {
        private const int DEFAULT_VALUE = 0;
        
        private int _value = DEFAULT_VALUE;
        
        public virtual Id<TypeModifierId> Id => TypeModifierId.Addition;

        public int Apply(int value) => _value + value;
        public int Apply(int value, IAbilityValue mod) => value + mod.Value;
        public void Set(IAbilityValue value) => _value = value.Value;
        public void Reset() => _value = DEFAULT_VALUE;
        public void Add(IAbilityValue value) => _value += value.Value;
        public void Remove(IAbilityValue value) => _value -= value.Value;
    }

    public class AbilityModRandom : IAbilityModifier
    {
        private const int DEFAULT_VALUE = 0, DEFAULT_CHANCE = 0;

        private int _value = DEFAULT_VALUE;
        private Chance _chance = DEFAULT_CHANCE;

        public virtual Id<TypeModifierId> Id => TypeModifierId.RandomAdd;

        public int Apply(int value) => value + (_chance.Roll ? _value : 0);
        public int Apply(int value, IAbilityValue mod) => value + (mod.Chance.Roll ? mod.Value : 0);

        public void Set(IAbilityValue value)
        {
            _value = value.Value;
            _chance = value.Chance;
        }
        public void Reset()
        {
            _value = DEFAULT_VALUE;
            _chance = DEFAULT_CHANCE;
        }
        public void Add(IAbilityValue value)
        {
            _value += value.Value;
            _chance.Add(value.Chance);
        }
        public void Remove(IAbilityValue value)
        {
            _value -= value.Value;
            _chance.Remove(value.Chance);
        }
    }

    public class AbilityModPercent : IAbilityModifier
    {
        private const int DEFAULT_VALUE = 100;

        private int _value = DEFAULT_VALUE;

        public Id<TypeModifierId> Id => TypeModifierId.Percent;

        public int Apply(int value) => value * _value / 100;
        public int Apply(int value, IAbilityValue mod) => value * mod.Value / 100;
        public void Set(IAbilityValue value) => _value = value.Value;
        public void Reset() => _value = DEFAULT_VALUE;
        public void Add(IAbilityValue value) => _value += value.Value;
        public void Remove(IAbilityValue value) => _value -= value.Value;
    }

    
}
