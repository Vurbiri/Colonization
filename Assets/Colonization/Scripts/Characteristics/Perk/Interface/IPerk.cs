namespace Vurbiri.Colonization.Characteristics
{
    public interface IPerk : IAbilityModifierSettings
    {
        public int TargetAbility { get; }
    }
}
