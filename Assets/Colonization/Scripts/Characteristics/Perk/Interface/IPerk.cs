//Assets\Colonization\Scripts\Characteristics\Perk\Interface\IPerk.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IPerk : IAbilityModifierValue
    {
        public int TargetAbility { get; }
    }
}
