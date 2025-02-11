//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Simple\PermanentTargetEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class PermanentTargetEffect : AEffect
    {
        public PermanentTargetEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier)
        {
            _value = isNegative ? -value : value;
        }

        public override int Apply(Actor self, Actor target) => target.ApplyEffect(this);
    }
}
