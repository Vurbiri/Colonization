using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class SelfTemporaryEffect : ATemporaryEffect
    {
        public SelfTemporaryEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration) : 
                                base(code, targetAbility, typeModifier, value, duration)
        {
        }

        public override int Apply(Actor self, Actor target) => self.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration));
    }
}
