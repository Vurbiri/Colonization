namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityModifierSettings : IAbilityValue
    {
        public Id<TypeModifierId> TypeModifier { get; }

    }
}
