//Assets\Colonization\Scripts\Characteristics\Effects\Abstract\AEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AEffect : IPerk
    {
        protected readonly int _targetAbility;
        protected readonly Id<TypeModifierId> _typeModifier;
        protected int _value;

        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;

        public AEffect(int targetAbility, Id<TypeModifierId> typeModifier)
        {
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
        }

        public AEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value)
        {
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
        }

        public abstract int Apply(Actor self, Actor target);
    }
}
