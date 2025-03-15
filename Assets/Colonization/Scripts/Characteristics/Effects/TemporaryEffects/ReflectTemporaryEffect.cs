//Assets\Colonization\Scripts\Characteristics\Effects\TemporaryEffects\ReflectTemporaryEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class ReflectTemporaryEffect : ATemporaryEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public ReflectTemporaryEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int reflectValue, int duration) :
                                base(code, targetAbility, typeModifier, value, duration)
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
