namespace Vurbiri.Colonization.Characteristics
{
    sealed public class ReflectHolyAttackEffect : HolyAttackEffect
    {
        private readonly AbilityModifierPercent _reflect;

        public ReflectHolyAttackEffect(int value, int holy, int pierce, int reflect) : base(value, holy, pierce) => _reflect = new(-reflect);

        public override int Apply(Actor self, Actor target)
        {
            _value = _reflect.Apply(base.Apply(self, target));
            return self.ApplyEffect(this);
        }
    }
}
