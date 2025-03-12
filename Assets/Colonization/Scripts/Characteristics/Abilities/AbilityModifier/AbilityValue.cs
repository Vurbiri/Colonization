//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\AbilityValue.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class AbilityValue : IAbilityValue
    {
        public Id<TypeModifierId> TypeModifier { get; }
        public int Value { get; }

        public AbilityValue(Id<TypeModifierId> type, int value)
        {
            TypeModifier = type;
            Value = value;
        }
    }
}
