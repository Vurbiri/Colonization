//Assets\Colonization\Scripts\Characteristics\Abilities\AbilityModifier\Interface\IAbilityModifier.cs
namespace Vurbiri.Colonization.Characteristics
{
    public interface IAbilityModifier : IValueId<TypeModifierId>
    {
        public int Value { get; set; }

        public int Apply(int value);
        public int Apply(int value, int modifier);

        public void Add(int value);
    }
}
