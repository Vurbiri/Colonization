//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Attacks\AttackEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class AttackEffect : AEffect
    {
        private readonly AbilityModifierPercent _usedModifier, _defModifier;

        public AttackEffect(int value, int _defenseValue) : base(ActorAbilityId.CurrentHP, TypeModifierId.Addition)
        {
            _usedModifier = new(-value);
            _defModifier = new(-_defenseValue);
        }

        public override int Apply(Actor self, Actor target)
        {
            int value = _usedModifier.Apply(self.Abilities[ActorAbilityId.Attack].Value);
            _value = -System.Math.Max(value + _defModifier.Apply(target.Abilities[ActorAbilityId.Defense].Value), 0);
            return target.ApplyEffect(this);
        }
    }
}
