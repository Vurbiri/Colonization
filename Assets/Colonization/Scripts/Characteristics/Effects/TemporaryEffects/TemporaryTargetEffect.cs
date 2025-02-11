//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\TemporaryTargetEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class TemporaryTargetEffect : ATemporaryEffect
    {
        public TemporaryTargetEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value, int duration, EffectCode _code) :
                                base(targetAbility, isNegative, typeModifier, value, duration, _code)
        {
        }

        public override int Apply(Actor self, Actor target)
        {
            target.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration));

            return 0;
        }
    }
}
