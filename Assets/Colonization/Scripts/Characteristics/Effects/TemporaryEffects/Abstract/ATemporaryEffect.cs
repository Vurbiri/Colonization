namespace Vurbiri.Colonization.Characteristics
{

    public abstract class ATemporaryEffect : AEffect
    {
        protected readonly int _duration;
        protected readonly bool _isNegative;

        protected ATemporaryEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value, int duration) : 
            base(targetAbility, typeModifier)
        {
            _value = isNegative ? -value : value;
            _duration = duration;
            _isNegative = isNegative;
        }

        
	}
}
