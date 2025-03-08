//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Heals\Abstract\AHealEffect.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AHealEffect : AHitEffect
    {
        private readonly AbilityModifierPercent _usedModifier;

        public AHealEffect(int value) : base(ActorAbilityId.CurrentHP, TypeModifierId.Addition)
        {
            _usedModifier = new(value);
        }

        protected void Init(IReadOnlyAbilities<ActorAbilityId> self)
        {
            _value = _usedModifier.Apply(self[ActorAbilityId.Attack].Value);
        }
    }
}
