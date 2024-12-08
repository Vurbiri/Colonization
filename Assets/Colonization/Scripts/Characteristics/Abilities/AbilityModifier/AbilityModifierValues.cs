//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\AbilityModifierValues.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class AbilityAddValue : IAbilityModifierValue
    {
        private readonly int _value;

        public Id<TypeModifierId> TypeModifier => TypeModifierId.Addition;
        public Chance Chance => 100;
        public int Value => _value;

        public AbilityAddValue(int value)
        {
            _value = value;
        }
    }

    public class AbilityRandomValue : IAbilityModifierValue
    {
        private readonly int _value;
        private readonly Chance _chance;

        public Id<TypeModifierId> TypeModifier => TypeModifierId.RandomAdd;
        public Chance Chance => _chance;
        public int Value => _value;

        public AbilityRandomValue(int value, Chance chance)
        {
            _value = value;
            _chance = chance;
        }
    }

    public class AbilityPercentValue : IAbilityModifierValue
    {
        private readonly int _value;

        public Id<TypeModifierId> TypeModifier => TypeModifierId.Percent;
        public Chance Chance => 100;
        public int Value => _value;

        public AbilityPercentValue(int value)
        {
            _value = value;
        }
    }
}
