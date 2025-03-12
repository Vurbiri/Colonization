//Assets\Colonization\Scripts\Characteristics\Perk\Interface\IPerk.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IPerk : IAbilityValue
    {
        public int TargetAbility { get; }
    }
}
