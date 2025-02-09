//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\Interface\IAbilityModifierValue.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityModifierValue
    {
        public Id<TypeModifierId> TypeModifier { get; }
        public int Value { get; }
    }
}
