namespace Vurbiri.Colonization
{
    sealed public class ApplyTargetEffect : AApplyHitEffect
    {
        public ApplyTargetEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier, value) { }

        public override int Apply(Actor self, Actor target) => target.ApplyEffect(this);
    }
}
