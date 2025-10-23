namespace Vurbiri.Colonization.Characteristics
{
    using static ActorAbilityId;

    public class HolyAttackEffect : AApplyHitEffect
    {
        private readonly AbilityModifierPercent _damageToHuman, _damageToDemon;
        private readonly AbilityModifierPercent _pierce;

        public HolyAttackEffect(int value, int holy, int pierce) : base(CurrentHP, TypeModifierId.Addition)
        {
            _damageToHuman = new(-value);
            _damageToDemon = new(-value + holy);
            _pierce = new(100 - pierce);
        }

        public override int Apply(Actor self, Actor target)
        {
            var damage = self.TypeId == target.TypeId ? _damageToHuman : _damageToDemon;

            _pierce.Add(-self.Abilities[Pierce].Value);
            _value = -Formulas.Damage(damage.Apply(self.Abilities[Attack].Value), _pierce.Apply(target.Abilities[Defense].Value));
            _pierce.Add(self.Abilities[Pierce].Value);

            return target.ApplyEffect(this);
        }
    }
}
