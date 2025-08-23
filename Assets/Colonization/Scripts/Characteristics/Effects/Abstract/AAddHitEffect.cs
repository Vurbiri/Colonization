namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AAddHitEffect : AApplyHitEffect
    {
        protected readonly int _duration;
        protected readonly EffectCode _code;

        protected AAddHitEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration) : base(targetAbility, typeModifier, value)
        {
            _duration = duration;
            _code = code;
        }
	}
}
