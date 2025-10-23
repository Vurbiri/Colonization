namespace Vurbiri.Colonization
{
    sealed public class AddReflectEffect : AAddHitEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public AddReflectEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int reflectValue, int duration) :
                                base(code, targetAbility, typeModifier, value, duration)
        {
            _reflectMod = new(reflectValue);
        }

        public override int Apply(Actor self, Actor target)
        {
            int temp = _value;
            _value = _reflectMod.Apply(target.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration, self.Owner != target.Owner)));
            self.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration, 0));
            return _value = temp;
        }
    }
}
