//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\SelfTemporaryEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class SelfTemporaryEffect : ATemporaryEffect
    {
        public SelfTemporaryEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, EffectCode code) : 
                                base(targetAbility, typeModifier, value, duration, code)
        {
        }

        public override int Apply(Actor self, Actor target) => self.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration));
    }
}
