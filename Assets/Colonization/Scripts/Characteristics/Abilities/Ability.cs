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
                _mods[TypeModifierId.Addition].Set(new AbilityValue(value - _baseValue));
                ApplyMods();
            }

        }
        public bool IsValue
        {
            get => _currentValue > 0;
            set
            {
                _mods[TypeModifierId.Addition].Set(new AbilityValue((value ? 1 : 0) - _baseValue));
                ApplyMods();
            }

        }
        public int NextValue
        {
            get
            {
                ApplyMods();
                return _currentValue;
            }
        }
        public Func<int, int> Clamp { set => funcClamp = value; }

        public Ability(Id<TId> id, int baseValue)
        {
            _mods[TypeModifierId.Addition] = new AbilityModAdd();
            _mods[TypeModifierId.RandomAdd] = new AbilityModRandom();
            _mods[TypeModifierId.Percent] = new AbilityModPercent();

            _id = id;
            _baseValue = _currentValue = baseValue;
        }

        public int Add(Id<TypeModifierId> id, AbilityValue value)
        {
            _mods[id].Add(value);
            return ApplyMods();
        }
        public int Remove(Id<TypeModifierId> id, AbilityValue value)
        {
            _mods[id].Remove(value);
            return ApplyMods();
        }

        public int Apply(Id<TypeModifierId> id, AbilityValue value) => funcClamp(_mods[id].Apply(_currentValue, value));

        public int AddModifier(IAbilityModifierValue settings)
        {
            _mods[settings.TypeModifier].Add(settings);
            return ApplyMods();
        }
        public int RemoveModifier(IAbilityModifierValue settings)
        {
            _mods[settings.TypeModifier].Remove(settings);
            return ApplyMods();
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

        private int ApplyMods()
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
