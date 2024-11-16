namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityModifier : IValueId<TypeModifierId>
    {
        public int Apply(int value);
        public int Apply(int value, IAbilityValue mod);

        public void Set(IAbilityValue value);
        public void Reset();

        public void Add(IAbilityValue value);
        public void Remove(IAbilityValue value);
    }
}
