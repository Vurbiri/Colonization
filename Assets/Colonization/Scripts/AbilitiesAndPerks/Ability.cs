using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Ability : AReactive<int>, IValueId<PlayerAbilityId>
    {
        private readonly Id<PlayerAbilityId> _id;
        private readonly int _baseValue;

        private int _currentValue;
        private readonly HashSet<IPerk<PlayerAbilityId>> _permanentPerks;
        private readonly HashSet<IPerk<PlayerAbilityId>> _randomPerks;

        public Id<PlayerAbilityId> Id => _id;
        public int CurrentValue => _currentValue;
        public int NextValue 
        { 
            get 
            { 
                if(_randomPerks.Count == 0)
                    return _currentValue;

                int newValue = _currentValue;
                foreach(IPerk<PlayerAbilityId> perk in _randomPerks)
                    newValue = perk.Apply(newValue);

                return newValue;
            } 
        }

        public Ability(int id, int baseValue)
        {
            _id = new(id);
            _baseValue = _currentValue = baseValue;
            _permanentPerks = new();
            _randomPerks = new();
        }

        public Ability(Ability ability)
        {
            _id = ability._id;
            _baseValue = _currentValue = ability._baseValue;
            _permanentPerks = new();
            _randomPerks = new();
        }

        public bool TryAddPerk(IPerk<PlayerAbilityId> perk)
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

        //private void Update()
        //{
        //    _currentValue = _baseValue;
        //    foreach(IPerk perk in _permanentPerks)
        //        _currentValue = perk.Apply(_currentValue);

        //    EventThisChange?.Invoke(this);
        //}
    }
}
