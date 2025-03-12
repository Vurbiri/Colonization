//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\Abstract\ATemporaryEffect.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ATemporaryEffect : AHitEffect
    {
        protected readonly int _duration;
        protected readonly EffectCode _code;

        protected ATemporaryEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration) : 
            base(targetAbility, typeModifier, value)
        {
            _duration = duration;
            _code = code;
        }
	}
}
