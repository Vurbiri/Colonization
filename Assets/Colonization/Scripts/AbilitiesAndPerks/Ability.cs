using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Ability : AReactive<int>, IValueTypeEnum<PlayerAbilityType>
    {
        private readonly PlayerAbilityType _type;
        private readonly int _baseValue;

        private int _currentValue;
        private readonly HashSet<IPerk> _permanentPerks;
        private readonly HashSet<IPerk> _randomPerks;

        public PlayerAbilityType Type => _type;
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

        public Ability(PlayerAbilityType type, int baseValue)
        {
            _type = type;
            _baseValue = _currentValue = baseValue;
            _permanentPerks = new();
            _randomPerks = new();
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
