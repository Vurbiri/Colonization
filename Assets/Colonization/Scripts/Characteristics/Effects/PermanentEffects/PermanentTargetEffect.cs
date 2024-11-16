namespace Vurbiri.Colonization.Characteristics
{
    using Actors;

    public class PermanentTargetEffect : PermanentSelfEffect
    {
        public PermanentTargetEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value) :
                                base(targetAbility, isNegative, typeModifier, value)
        {
        }

        public override void Apply(Actor self, Actor target) => target.ApplyEffect(this);
    }
}
