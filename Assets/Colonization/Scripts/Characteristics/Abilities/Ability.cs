//Assets\Colonization\Scripts\Characteristics\Abilities\Ability.cs
using System;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class Ability<TId> : IReadOnlyReactive<int>, IValueId<TId> where TId : AAbilityId<TId>
    {
        private readonly Id<TId> _id;
        private readonly int _baseValue;
        private int _currentValue;

        private readonly IdArray<TypeModifierId, IAbilityModifier> _mods = new();

        private Func<int, int> funcClamp = v => Math.Max(v, 0);
        private Action<int> actionValueChange;

        public Id<TId> Id => _id;

        public int BaseValue => _baseValue;

        public int Value
        {
            get => _currentValue;
            set
            {
                _mods[TypeModifierId.Addition].Value = value - _baseValue;
                _mods[TypeModifierId.Percent].Reset();
                ApplyModifiers();
            }

        }
        public bool IsValue
        {
            get => _currentValue > 0;
            set
            {
                _mods[TypeModifierId.Addition].Value = (value ? 1 : 0) - _baseValue;
                _mods[TypeModifierId.Percent].Reset();
                ApplyModifiers();
            }

        }

        public Func<int, int> Clamp { set => funcClamp = value; }

        public Ability(Id<TId> id, int baseValue)
        {
            _mods[TypeModifierId.Addition] = new AbilityModifierAdd();
            _mods[TypeModifierId.Percent] = new AbilityModifierPercent();

            _id = id;
            _baseValue = _currentValue = baseValue;
        }

        public int Add(Id<TypeModifierId> id, int value)
        {
            _mods[id].Add(value);
            return ApplyModifiers();
        }

        public int ApplyModifier(Id<TypeModifierId> id, int value) => funcClamp(_mods[id].Apply(_currentValue, value));

        public int AddModifier(IAbilityModifierValue mod)
        {
            _mods[mod.TypeModifier].Add(mod.Value);
            return ApplyModifiers();
        }
        public int RemoveModifier(IAbilityModifierValue mod)
        {
            _mods[mod.TypeModifier].Add(-mod.Value);
            return ApplyModifiers();
        }
        
        public void Reset()
        {
            for (int i = 0;i < TypeModifierId.Count;i++)
                _mods[i].Reset();
        }

        public IUnsubscriber Subscribe(Action<int> action, bool calling = true)
        {
            actionValueChange += action;
            if (calling)
                action(_currentValue);

            return new Unsubscriber<Action<int>>(this, action);
        }

        public void Unsubscribe(Action<int> action) => actionValueChange -= action;

        private int ApplyModifiers()
        {
            int old = _currentValue;
            _currentValue = _baseValue;
            for (int i = 0; i < TypeModifierId.Count; i++)
                _currentValue = _mods[i].Apply(_currentValue);
            _currentValue = funcClamp(_currentValue);

            actionValueChange?.Invoke(_currentValue);
            return _currentValue - old;
        }
    }
}
