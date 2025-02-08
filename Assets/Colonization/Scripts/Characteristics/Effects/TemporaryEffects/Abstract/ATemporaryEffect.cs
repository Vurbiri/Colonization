//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\Abstract\ATemporaryEffect.cs
namespace Vurbiri.Colonization.Characteristics
{

    public abstract class ATemporaryEffect : AEffect
    {
        protected readonly int _duration;
        protected readonly bool _isNegative;
        protected readonly EffectCode _code;

        protected ATemporaryEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value, int duration, EffectCode code) : 
            base(targetAbility, typeModifier)
        {
            _value = isNegative ? -value : value;
            _duration = duration;
            _isNegative = isNegative;
            _code = code;
        }

        
	}
}
