//Assets\Colonization\Scripts\Characteristics\Effects\HitEffects\Abstract\AHitEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AHitEffect : Perk
    {
        public AHitEffect(int targetAbility, Id<TypeModifierId> typeModifier) : base(targetAbility, typeModifier, 0) { }
        public AHitEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier, value) { }

        public abstract int Apply(Actor self, Actor target);
    }
}
