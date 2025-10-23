namespace Vurbiri.Colonization
{
    public abstract class AHealEffect : AApplyHitEffect
    {
        private readonly AbilityModifierPercent _usedModifier;

        public AHealEffect(int value) : base(ActorAbilityId.CurrentHP, TypeModifierId.Addition)
        {
            _usedModifier = new(value);
        }

        protected void Init(ReadOnlyAbilities<ActorAbilityId> self)
        {
            _value = _usedModifier.Apply(self[ActorAbilityId.Attack].Value);
        }
    }
}
