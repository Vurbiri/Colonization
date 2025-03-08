//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\SelfEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class SelfEffect : AHitEffect
    {
        public SelfEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier, value) { }

        public override int Apply(Actor self, Actor target) => self.ApplyEffect(this);
    }
}
