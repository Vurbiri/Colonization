namespace Vurbiri.Colonization.Characteristics
{
    sealed public class AddSelfEffect : AAddHitEffect
    {
        public AddSelfEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration) : 
                                base(code, targetAbility, typeModifier, value, duration)
        {
        }

        public override int Apply(Actor self, Actor target) => self.AddEffect(new(_code, _targetAbility, _typeModifier, _value, _duration, 0));
    }
}
