//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Simple\PermanentSelfEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class PermanentSelfEffect : AEffect
    {
        public PermanentSelfEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier)
        {
            _value = isNegative ? -value : value;
        }

        public override int Apply(Actor self, Actor target) => self.ApplyEffect(this);
    }
}
