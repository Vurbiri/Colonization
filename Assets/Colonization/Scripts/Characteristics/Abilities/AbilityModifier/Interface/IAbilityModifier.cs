namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityModifier : IValueId<TypeModifierId>
    {
        public int Value { get; set; }

        public int Apply(int value);

        public void Add(int value);
    }
}
