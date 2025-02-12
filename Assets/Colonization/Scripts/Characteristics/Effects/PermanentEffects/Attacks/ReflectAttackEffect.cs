//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Attacks\ReflectAttackEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReflectAttackEffect : AttackEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public ReflectAttackEffect(int value, int reflectValue) :  base( value) 
        {
            _reflectMod = new(reflectValue);
        }

        public override int Apply(Actor self, Actor target)
        {
            _value = _reflectMod.Apply(base.Apply(self, target));
            return self.ApplyEffect(this);
        }
    }
}
