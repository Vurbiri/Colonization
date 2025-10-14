using System;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public class Ability<TId> : AAbility<TId> where TId : AbilityId<TId>
    {
        private readonly IdArray<TypeModifierId, AbilityModifier> _modifiers = new();
        protected readonly int _baseValue;

        public Ability(Id<TId> id, int baseValue) : base(id, baseValue)
        {
            _baseValue = baseValue;

            _modifiers[TypeModifierId.BasePercent]  = new AbilityModifierPercent();
            _modifiers[TypeModifierId.Addition]     = new AbilityModifierAdd();
            _modifiers[TypeModifierId.TotalPercent] = new AbilityModifierPercent();
        }

        sealed public override int AddModifier(IAbilityValue mod)
        {
            _modifiers[mod.TypeModifier].Add(mod.Value);
            return ApplyModifiers();
        }
        sealed public override int RemoveModifier(IAbilityValue mod)
        {
            _modifiers[mod.TypeModifier].Add(-mod.Value);
            return ApplyModifiers();
        }

        private int ApplyModifiers()
        {
            int old = _value;

            _value = _baseValue;
            for (int i = 0; i < TypeModifierId.Count; i++)
                _value = _modifiers[i].Apply(_value);

            _value = Math.Max(_value, 0);

            if (old != _value) 
                _changeEvent.Invoke(_value);

            return _value - old;
        }
    }
}
