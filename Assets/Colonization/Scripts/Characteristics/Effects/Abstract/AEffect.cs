//Assets\Colonization\Scripts\Characteristics\Effects\Abstract\AEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AEffect : Perk
    {
        public AEffect(int targetAbility, Id<TypeModifierId> typeModifier) : base(targetAbility, typeModifier, 0) { }
        public AEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier, value) { }

        public abstract int Apply(Actor self, Actor target);
    }
}
