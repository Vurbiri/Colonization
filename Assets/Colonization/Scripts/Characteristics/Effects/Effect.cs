namespace Vurbiri.Colonization.Characteristics
{
    public class Effect : IPerk
    {
        protected readonly int _targetAbility;
        protected readonly Id<TypeModifierId> _typeModifier;
        protected int _value;

        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;

        public Effect(int targetAbility, Id<TypeModifierId> typeModifier, int value)
        {
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
        }

        public void Add(IPerk perk)
        {
            if (perk == null || _targetAbility != perk.TargetAbility | _typeModifier != perk.TypeModifier)
                Errors.Argument("Perk", perk);

            _value += perk.Value;
        }

        public override string ToString() => $"{GetType().Name}: targetID = {_targetAbility}, modifierID = {_typeModifier}, value = {_value}.";
    }
}
