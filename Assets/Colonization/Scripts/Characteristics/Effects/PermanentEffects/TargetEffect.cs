//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\TargetEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class TargetEffect : AEffect
    {
        public TargetEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier, value) { }

        public override int Apply(Actor self, Actor target) => target.ApplyEffect(this);
    }
}
