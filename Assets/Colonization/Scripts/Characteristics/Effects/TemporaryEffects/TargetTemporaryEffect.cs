//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\TargetTemporaryEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class TargetTemporaryEffect : ATemporaryEffect
    {
        public TargetTemporaryEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration) :
                                base(code, targetAbility, typeModifier, value, duration)
        {
        }

        public override int Apply(Actor self, Actor target) => target.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration));
    }
}
