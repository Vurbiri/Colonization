//Assets\Colonization\Scripts\Characteristics\Perk\Interface\IPerk.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IPerk : IAbilityModifierSettings
    {
        public int TargetAbility { get; }
    }
}
