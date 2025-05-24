namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityValue
    {
        public Id<TypeModifierId> TypeModifier { get; }
        public int Value { get; }
    }
}
