namespace Vurbiri.Colonization
{
    public interface IAbilityModifierSettings
    {
        public Id<TypeOperationId> TypeOperation { get; }
        public int Value { get; }
        public Chance Chance { get; }
    }
}
