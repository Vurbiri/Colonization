//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Abstract\APermanentUsedNotContrEffect.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class APermanentUsedNotCounterEffect : AEffect
    {
        private readonly IAbilityModifier _usedModifier;
        private readonly Id<ActorAbilityId> _usedAbility;
        private readonly bool _isNegative;

        public APermanentUsedNotCounterEffect(int targetAbility, bool isNegative, int usedAbility, Id<TypeModifierId> typeModifier, int value) :
                               base(targetAbility, TypeModifierId.Addition)
        {
            _usedModifier = AbilityModifierFactory.Create(typeModifier, value);
            _usedAbility = usedAbility;
            _isNegative = isNegative;
        }

        protected void Init(AbilitiesSet<ActorAbilityId> self)
        {
            _value = _usedModifier.Apply(self.GetValue(_usedAbility)) * (_isNegative ? -1 : 1);
        }
    }
}
