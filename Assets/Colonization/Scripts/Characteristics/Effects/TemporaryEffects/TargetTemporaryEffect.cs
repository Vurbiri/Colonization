//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\TargetTemporaryEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class TargetTemporaryEffect : ATemporaryEffect
    {
        public TargetTemporaryEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, EffectCode _code) :
                                base(targetAbility, typeModifier, value, duration, _code)
        {
        }

        public override int Apply(Actor self, Actor target) => target.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration));
    }
}
