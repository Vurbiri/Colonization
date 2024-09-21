using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class Ability : AReactive<int>
    {
        [SerializeField] private AbilityType _type;
        [SerializeField] private int _baseValue;

        private int _currentValue;
        private readonly HashSet<IPerk> _permanentPerks;
        private readonly HashSet<IPerk> _randomPerks;

        public AbilityType Type => _type;
        public int CurrentValue => _currentValue;
        public int NextValue 
        { 
            get 
            { 
                if(_randomPerks.Count == 0)
                    return _currentValue;

                int newValue = _currentValue;
                foreach(IPerk perk in _randomPerks)
                    newValue = perk.Apply(newValue);

                return newValue;
            } 
        }

        public Ability(Ability ability)
        {
            _type = ability._type;
            _baseValue = _currentValue = ability._baseValue;
            _permanentPerks = new();
            _randomPerks = new();
        }

        public bool TryAddPerk(IPerk perk)
        {
            if (perk.IsPermanent)
            {
                if (_permanentPerks.Add(perk))
                {
                    _currentValue = perk.Apply(_currentValue);
                    EventThisChange?.Invoke(_currentValue);
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
