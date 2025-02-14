//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Attacks\AttackNotDefEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class AttackNotDefEffect : AEffect
    {
        private readonly AbilityModifierPercent _usedModifier;

        public AttackNotDefEffect(int value) : base(ActorAbilityId.CurrentHP, TypeModifierId.Addition) => _usedModifier = new(value);

        public override int Apply(Actor self, Actor target)
        {
            _value = _usedModifier.Apply(self.Abilities[ActorAbilityId.Attack].Value);
            return target.ApplyEffect(this);
        }
    }
}
