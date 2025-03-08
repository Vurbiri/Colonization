//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\Abstract\ATemporaryEffect.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ATemporaryEffect : AHitEffect
    {
        protected readonly int _duration;
        protected readonly bool _isNegative;
        protected readonly EffectCode _code;

        protected ATemporaryEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, EffectCode code) : 
            base(targetAbility, typeModifier, value)
        {
            _duration = duration;
            _code = code;
        }
	}
}
