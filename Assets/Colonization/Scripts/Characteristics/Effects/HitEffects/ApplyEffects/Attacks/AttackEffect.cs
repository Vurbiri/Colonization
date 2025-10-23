namespace Vurbiri.Colonization.Characteristics
{
    using static ActorAbilityId;

    public class AttackEffect : AApplyHitEffect
    {
        private readonly AbilityModifierPercent _pierce;
        private readonly AbilityModifierPercent _damage;

        public AttackEffect(int value, int pierce) : base(CurrentHP, TypeModifierId.Addition)
        {
            _damage = new(-value);
            _pierce = new(100 - pierce);
        }

        public override int Apply(Actor self, Actor target)
        {
            _pierce.Add(-self.Abilities[Pierce].Value);
            _value = -Formulas.Damage(_damage.Apply(self.Abilities[Attack].Value), _pierce.Apply(target.Abilities[Defense].Value));
            _pierce.Add(self.Abilities[Pierce].Value);

            return target.ApplyEffect(this);
        }
    }
}
