//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\TemporaryReflectEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class TemporaryReflectEffect : ATemporaryEffect
    {
        public TemporaryReflectEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value, int duration, EffectCode _code) :
                                base(targetAbility, isNegative, typeModifier, value, duration, _code)
        {
        }

        public override void Apply(Actor self, Actor target)
        {
            target.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration));
            self.AddEffect(new(_code, _targetAbility, _typeModifier, -_value, _duration));
        }
    }
}
