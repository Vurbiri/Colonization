namespace Vurbiri.Colonization.Characteristics
{

    public abstract class ATemporaryEffect : AEffect
    {
        protected readonly EffectCode _code;
        protected readonly int _duration;
        protected readonly bool _isNegative;

        protected ATemporaryEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value, int duration, EffectCode _code) : 
            base(targetAbility, typeModifier)
        {
            _value = isNegative ? -value : value;
            _duration = duration;
            _isNegative = isNegative;
        }

        
	}
}
