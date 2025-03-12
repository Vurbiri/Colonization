//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\Interface\IAbilityValue.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityValue
    {
        public Id<TypeModifierId> TypeModifier { get; }
        public int Value { get; }
    }
}
