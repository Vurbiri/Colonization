//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Heals\Abstract\AAttackNotDefenseEffect.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AAttackNotDefenseEffect : AEffect
    {
        private readonly AbilityModifierPercent _usedModifier;

        public AAttackNotDefenseEffect(int value) : base(ActorAbilityId.CurrentHP, TypeModifierId.Addition)
        {
            _usedModifier = new(value);
        }

        protected void Init(IReadOnlyAbilities<ActorAbilityId> self)
        {
            _value = _usedModifier.Apply(self[ActorAbilityId.Attack].Value);
        }
    }
}
