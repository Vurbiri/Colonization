//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Attacks\ReflectAttackNotDefEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReflectAttackNotDefEffect : AttackNotDefEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public ReflectAttackNotDefEffect(int value, int reflectValue) : base(value) => _reflectMod = new(-reflectValue);

        public override int Apply(Actor self, Actor target)
        {
            _value = _reflectMod.Apply(base.Apply(self, target));
            return self.ApplyEffect(this);
        }
    }
}
