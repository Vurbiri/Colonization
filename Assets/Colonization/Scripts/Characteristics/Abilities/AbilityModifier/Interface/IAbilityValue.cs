namespace Vurbiri.Colonization
{
    public interface IAbilityValue
    {
        public Id<TypeModifierId> TypeModifier { get; }
        public int Value { get; }
    }
}
