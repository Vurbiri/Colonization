using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class AAbility<T> : AReactive<int>, IValueId<T> where T : AAbilityId<T>
    {
        private readonly Id<T> _id;
        private readonly int _baseValue;

        private int _currentValue;
        private readonly HashSet<IPerk<T>> _permanentPerks;
        private readonly HashSet<IPerk<T>> _randomPerks;

        public Id<T> Id => _id;
        public int CurrentValue => _currentValue;
        public int NextValue
        {
            get
            {
                if (_randomPerks.Count == 0)
                    return _currentValue;

                int newValue = _currentValue;
                foreach (IPerk<T> perk in _randomPerks)
                    newValue = perk.Apply(newValue);

                return newValue;
            }
        }

        public AAbility(Id<T> id, int baseValue)
        {
            _id = id;
            _baseValue = _currentValue = baseValue;
            _permanentPerks = new();
            _randomPerks = new();
        }

        public AAbility(AAbility<T> ability)
        {
            _id = ability._id;
            _baseValue = _currentValue = ability._baseValue;
            _permanentPerks = new();
            _randomPerks = new();
        }

        public bool TryAddPerk(IPerk<T> perk)
        {
            if (perk.IsPermanent)
            {
                if (_permanentPerks.Add(perk))
                {
                    _currentValue = perk.Apply(_currentValue);
                    ActionThisChange?.Invoke(_currentValue);
                    return true;
                }

                return false;
            }

            return _randomPerks.Add(perk);
        }

        protected override void Callback(Action<int> action) => action(_currentValue);
    }
}
