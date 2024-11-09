namespace Vurbiri.Colonization
{
    public class AbilityModAdd : IAbilityModifier
    {
        private int _value = 0;
        
        public virtual Id<TypeOperationId> Id => TypeOperationId.Addition;

        public int Apply(int value) => _value + value;
        public void Add(IAbilityModifierSettings settings) => _value += settings.Value;
        public void Remove(IAbilityModifierSettings settings) => _value -= settings.Value;
    }

    public class AbilityModRandom : IAbilityModifier
    {
        private int _value = 0;
        private Chance _chance = 0;

        public virtual Id<TypeOperationId> Id => TypeOperationId.RandomAdd;

        public int Apply(int value) => value + (_chance.Roll ? _value : 0);
        public void Add(IAbilityModifierSettings settings)
        {
            _value += settings.Value;
            _chance.Add(settings.Chance);
        }

        public void Remove(IAbilityModifierSettings settings)
        {
            _value -= settings.Value;
            _chance.Remove(settings.Chance);
        }
    }

    public class AbilityModPercent : IAbilityModifier
    {
        private int _value = 100;

        public Id<TypeOperationId> Id => TypeOperationId.Percent;

        public int Apply(int value) => value * _value / 100;
        public void Add(IAbilityModifierSettings settings) => _value += settings.Value;
        public void Remove(IAbilityModifierSettings settings) => _value -= settings.Value;
    }

    
}
