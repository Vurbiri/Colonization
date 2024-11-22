//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\AbilityModAddSettings.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class AbilityModAddSettings : IAbilityModifierSettings
    {
        private readonly int _value;

        public Id<TypeModifierId> TypeModifier => TypeModifierId.Addition;
        public Chance Chance => 100;
        public int Value => _value;

        public AbilityModAddSettings(int value)
        {
            _value = value;
        }
    }

    public class AbilityModRandomSettings : IAbilityModifierSettings
    {
        private readonly int _value;
        private readonly Chance _chance;

        public Id<TypeModifierId> TypeModifier => TypeModifierId.RandomAdd;
        public Chance Chance => _chance;
        public int Value => _value;

        public AbilityModRandomSettings(int value, Chance chance)
        {
            _value = value;
            _chance = chance;
        }
    }

    public class AbilityModPercentSettings : IAbilityModifierSettings
    {
        private readonly int _value;

        public Id<TypeModifierId> TypeModifier => TypeModifierId.Percent;
        public Chance Chance => 100;
        public int Value => _value;

        public AbilityModPercentSettings(int value)
        {
            _value = value;
        }
    }
}
