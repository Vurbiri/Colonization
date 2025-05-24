using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class ReflectHealEffect : TargetHealEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public ReflectHealEffect(int value, int reflectValue) : base(value)  => _reflectMod = new(-reflectValue);

        public override int Apply(Actor self, Actor target)
        {
            _value = _reflectMod.Apply(base.Apply(self, target));
            return self.ApplyEffect(this);
        }
    }
}
