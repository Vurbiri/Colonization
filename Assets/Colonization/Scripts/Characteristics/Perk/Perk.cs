//Assets\Colonization\Scripts\Characteristics\Perk\Perk.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class Perk : IPerk
    {
        protected readonly int _targetAbility;
        protected readonly Id<TypeModifierId> _typeModifier;
        protected int _value;

        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;

        public Perk(int targetAbility, Id<TypeModifierId> typeModifier, int value)
        {
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
        }
    }
}
