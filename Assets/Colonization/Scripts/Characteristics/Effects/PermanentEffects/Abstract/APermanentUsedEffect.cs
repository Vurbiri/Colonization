//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Abstract\APermanentUsedEffect.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class APermanentUsedEffect : AEffect
    {
        protected readonly Id<TypeModifierId> _typeUsedModifier;
        private readonly Id<ActorAbilityId> _usedAbility;
        private readonly Id<ActorAbilityId> _counteredAbility;
        private readonly AbilityValue _abilityValue;
        private readonly bool _isNegative;

        public APermanentUsedEffect(int targetAbility, bool isNegative, int usedAbility, int counteredAbility, Id<TypeModifierId> typeModifier, int value) :
                               base(targetAbility, TypeModifierId.Addition)
        {
            _typeUsedModifier = typeModifier;
            _usedAbility = usedAbility;
            _counteredAbility = counteredAbility;
            _abilityValue = value;
            _isNegative = isNegative;
        }

        protected void Init(AbilitiesSet<ActorAbilityId> self, AbilitiesSet<ActorAbilityId> target) 
        {
            int value = self.ApplyValue(_usedAbility, _typeUsedModifier, _abilityValue);
            value = System.Math.Max(value - target.GetValue(_counteredAbility), 0);
            _value = _isNegative ? -value : value;
        }
    }
}
