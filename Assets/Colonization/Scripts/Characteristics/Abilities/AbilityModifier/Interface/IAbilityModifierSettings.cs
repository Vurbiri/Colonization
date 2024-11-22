//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\Interface\IAbilityModifierSettings.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityModifierSettings : IAbilityValue
    {
        public Id<TypeModifierId> TypeModifier { get; }

    }
}
