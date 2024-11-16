using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class APermanentUsedEffect : AEffect
    {
        private readonly Id<ActorAbilityId> _usedAbility;
        private readonly Id<ActorAbilityId> _counteredAbility;
        private readonly AbilityValue _abilityValue;
        private readonly bool _isNegative;
        public APermanentUsedEffect(int targetAbility, bool isNegative, int usedAbility, int counteredAbility, Id<TypeModifierId> typeModifier, int value) :
                               base(targetAbility, typeModifier)
        {
            _usedAbility = usedAbility;
            _counteredAbility = counteredAbility;
            _abilityValue = value;
            _isNegative = isNegative;
        }

        public override void Init(AbilitiesSet<ActorAbilityId> self, AbilitiesSet<ActorAbilityId> target) 
        {
            int value = self.ApplyValue(_usedAbility, _typeModifier, _abilityValue);
            value = Mathf.Clamp(value - target.GetValue(_counteredAbility), 0 , int.MinValue);
            _value = _isNegative ? -value : value;
        }

    }
}
