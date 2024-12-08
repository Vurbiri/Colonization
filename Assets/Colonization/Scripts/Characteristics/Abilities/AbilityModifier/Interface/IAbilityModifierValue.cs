//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\Interface\IAbilityModifierValue.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityModifierValue : IAbilityValue
    {
        public Id<TypeModifierId> TypeModifier { get; }

    }
}
