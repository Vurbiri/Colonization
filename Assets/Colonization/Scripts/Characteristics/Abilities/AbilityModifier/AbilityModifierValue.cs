//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\AbilityModifierValue.cs
namespace Vurbiri.Colonization.Characteristics
{
    public readonly struct AbilityModifierValue : IAbilityModifierValue
    {
        public Id<TypeModifierId> TypeModifier { get; }
        public int Value { get; }

        public AbilityModifierValue(Id<TypeModifierId> type, int value)
        {
            TypeModifier = type;
            Value = value;
        }
    }
}
