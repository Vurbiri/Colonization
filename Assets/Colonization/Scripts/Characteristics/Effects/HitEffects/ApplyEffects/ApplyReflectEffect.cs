namespace Vurbiri.Colonization
{
    sealed public class ApplyReflectEffect : AApplyHitEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public ApplyReflectEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value, int reflectValue) : base(targetAbility, typeModifier, value) 
        {
            _reflectMod = new(reflectValue);
        }

        public override int Apply(Actor self, Actor target)
        {
            int temp = _value;
            _value = _reflectMod.Apply(target.ApplyEffect(this));
            self.ApplyEffect(this);
            return _value = temp;
        }
    }
}
