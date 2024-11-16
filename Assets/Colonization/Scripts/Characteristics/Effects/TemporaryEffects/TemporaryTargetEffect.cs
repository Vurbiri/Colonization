namespace Vurbiri.Colonization.Characteristics
{
    using Actors;

    public class TemporaryTargetEffect : ATemporaryEffect
    {
        public TemporaryTargetEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value, int duration) :
                                base(targetAbility, isNegative, typeModifier, value, duration)
        {
        }

        public override void Apply(Actor self, Actor target)
        {
            target.AddEffect(new(_targetAbility, _typeModifier, _value, _duration, _isNegative));
        }
    }
}
