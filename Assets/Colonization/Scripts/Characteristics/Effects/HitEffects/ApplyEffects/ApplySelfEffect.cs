namespace Vurbiri.Colonization.Characteristics
{
    sealed public class ApplySelfEffect : AApplyHitEffect
    {
        public ApplySelfEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier, value) { }

        public override int Apply(Actor self, Actor target) => self.ApplyEffect(this);
    }
}
