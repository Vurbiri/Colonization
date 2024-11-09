namespace Vurbiri.Colonization
{
    public interface IAbilityModifier : IValueId<TypeOperationId>
    {
        public int Apply(int value);
        public void Add(IAbilityModifierSettings settings);
        public void Remove(IAbilityModifierSettings settings);
    }
}
