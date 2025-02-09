//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Abstract\APermanentUsedNotContrEffect.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class APermanentUsedNotCounterEffect : AEffect
    {
        protected readonly Id<TypeModifierId> _typeUsedModifier;
        protected readonly Id<ActorAbilityId> _usedAbility;
        protected readonly int _abilityValue;
        protected readonly bool _isNegative;

        public APermanentUsedNotCounterEffect(int targetAbility, bool isNegative, int usedAbility, Id<TypeModifierId> typeModifier, int value) :
                               base(targetAbility, TypeModifierId.Addition)
        {
            _typeUsedModifier = typeModifier;
            _usedAbility = usedAbility;
            _abilityValue = value;
            _isNegative = isNegative;
        }

        protected void Init(AbilitiesSet<ActorAbilityId> self)
        {
            _value = self.ApplyValue(_usedAbility, _typeUsedModifier, _abilityValue) * (_isNegative ? -1 : 1);
        }
    }
}
