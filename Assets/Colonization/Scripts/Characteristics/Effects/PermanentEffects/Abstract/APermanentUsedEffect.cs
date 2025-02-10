//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Abstract\APermanentUsedEffect.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class APermanentUsedEffect : AEffect
    {
        private readonly IAbilityModifier _usedModifier;
        private readonly Id<ActorAbilityId> _usedAbility;
        private readonly Id<ActorAbilityId> _counteredAbility;
        private readonly bool _isNegative;

        public APermanentUsedEffect(int targetAbility, bool isNegative, int usedAbility, int counteredAbility, Id<TypeModifierId> typeModifier, int value) :
                               base(targetAbility, TypeModifierId.Addition)
        {
            _usedModifier = AbilityModifierFactory.Create(typeModifier, value);
            _usedAbility = usedAbility;
            _counteredAbility = counteredAbility;
            _isNegative = isNegative;
        }

        protected void Init(AbilitiesSet<ActorAbilityId> self, AbilitiesSet<ActorAbilityId> target) 
        {
            int value = _usedModifier.Apply(self.GetValue(_usedAbility));
            value = System.Math.Max(value - target.GetValue(_counteredAbility), 0);
            _value = _isNegative ? -value : value;
        }
    }
}
