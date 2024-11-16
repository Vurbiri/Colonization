namespace Vurbiri.Colonization.Characteristics
{
    using System;
    using Vurbiri.Collections;
    using Vurbiri.Reactive;

    public class Ability<TId> : IReadOnlyReactiveValue<int>, IValueId<TId> where TId : AAbilityId<TId>
    {
        private readonly Id<TId> _id;
        private readonly int _baseValue;
        private int _currentValue;
        private readonly IdArray<TypeModifierId, IAbilityModifier> _mods = new();
        private Func<int, int> funcClamp;
        private Action<int> actionValueChange;

        public Id<TId> Id => _id;

        public int BaseValue => _baseValue;

        public int Value
        {
            get => _currentValue;
            set
            {
                _mods[TypeModifierId.Addition].Add(new AbilityValue(value - _baseValue));
                ApplyMods();
            }

        }
        public bool IsValue
        {
            get => _currentValue > 0;
            set
            {
                _mods[TypeModifierId.Addition].Add(new AbilityValue((value ? 1 : 0) - _baseValue));
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

        public void Add(Id<TypeModifierId> id, AbilityValue value)
        {
            _mods[id].Add(value);
            ApplyMods();
        }
        public void Remove(Id<TypeModifierId> id, AbilityValue value)
        {
            _mods[id].Remove(value);
            ApplyMods();
        }

        public int Apply(Id<TypeModifierId> id, AbilityValue value) => _mods[id].Apply(_currentValue, value);

        public void AddModifier(IAbilityModifierSettings settings)
        {
            _mods[settings.TypeModifier].Add(settings);
            ApplyMods();
        }
        public void RemoveModifier(IAbilityModifierSettings settings)
        {
            _mods[settings.TypeModifier].Remove(settings);
            ApplyMods();
        }
        
        public void Reset()
        {
            for (int i = 0;i < TypeModifierId.Count;i++)
                _mods[i].Reset();
        }

        public Unsubscriber<int> Subscribe(Action<int> action, bool calling = true)
        {
            actionValueChange -= action ?? throw new ArgumentNullException("Action<int> action");

            actionValueChange += action;
            if (calling)
                action(_currentValue);

            return new(this, action);
        }

        public void Unsubscribe(Action<int> action) => actionValueChange -= action ?? throw new ArgumentNullException("Action<int> action");

        private void ApplyMods()
        {
            int old = _currentValue;
            _currentValue = _baseValue;
            for (int i = 0; i < TypeModifierId.Count; i++)
                _currentValue = _mods[i].Apply(_currentValue);

            if (funcClamp != null)
                _currentValue = funcClamp(_currentValue);

            if (old != _currentValue & actionValueChange != null)
                actionValueChange(_currentValue);
        }
    }
}
