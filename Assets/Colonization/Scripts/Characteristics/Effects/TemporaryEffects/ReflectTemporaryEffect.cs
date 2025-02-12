//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\ReflectTemporaryEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReflectTemporaryEffect : ATemporaryEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public ReflectTemporaryEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value, int reflectValue, int duration, EffectCode _code) :
                                base(targetAbility, typeModifier, value, duration, _code)
        {
            _reflectMod = new(reflectValue);
        }

        public override int Apply(Actor self, Actor target)
        {
            int temp = _value;
            _value = _reflectMod.Apply(target.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration)));
            self.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration));
            return _value = temp;
        }
    }
}
