using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class ReflectAttackEffect : AttackEffect
    {
        private readonly AbilityModifierPercent _reflect;

        public ReflectAttackEffect(int value, int pierce, int reflect) : base( value, pierce)  => _reflect = new(-reflect);

        public override int Apply(Actor self, Actor target)
        {
            _value = _reflect.Apply(base.Apply(self, target));
            return self.ApplyEffect(this);
        }
    }
}
