using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class TemporaryReflectEffect : ATemporaryEffect
    {
        public TemporaryReflectEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value, int duration) :
                                base(targetAbility, isNegative, typeModifier, value, duration)
        {
        }

        public override void Apply(Actor self, Actor target)
        {
            int value = -target.AddEffect(new(_targetAbility, _typeModifier, _value, _duration, _isNegative));
            self.AddEffect(new(_targetAbility, _typeModifier, value, _duration, !_isNegative));
        }
    }
}
