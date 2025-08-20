using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReflectAttackEffect : AttackEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public ReflectAttackEffect(int value, int defenseValue, int reflectValue) :  base( value, defenseValue)  => _reflectMod = new(-reflectValue);

        public override int Apply(Actor self, Actor target)
        {
            _value = _reflectMod.Apply(base.Apply(self, target));
            return self.ApplyEffect(this);
        }
    }
}
