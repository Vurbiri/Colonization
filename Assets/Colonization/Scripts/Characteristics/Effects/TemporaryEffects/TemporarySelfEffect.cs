//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\TemporarySelfEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class TemporarySelfEffect : ATemporaryEffect
    {
        public TemporarySelfEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value, int duration, EffectCode code) : 
                                base(targetAbility, isNegative, typeModifier, value, duration, code)
        {
        }

        public override void Apply(Actor self, Actor target)
        {
            self.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration));
        }
    }
}
