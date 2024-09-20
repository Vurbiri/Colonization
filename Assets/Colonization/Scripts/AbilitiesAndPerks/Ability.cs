using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class Ability : AReactiveValue<Ability>
    {
        [SerializeField] private AbilityType _type;
        [SerializeField] private int _baseValue;

        private int _currentValue;
        private readonly HashSet<IPerk> _perks;

        public AbilityType Type => _type;
        public int CurrentValue  => _currentValue;
        public int NewCurrentValue { get { Update();  return _currentValue; } }

        public Ability(Ability ability)
        {
            _type = ability._type;
            _baseValue = _currentValue = ability._baseValue;
            _perks = new();
        }

        public bool TryAddPerk(IPerk perk)
        {
            if(_perks.Add(perk))
            {
                _currentValue = perk.Apply(_currentValue);
                EventValueChange?.Invoke(this);
                return true;
            }

            return false;
        }

        private void Update()
        {
            _currentValue = _baseValue;
            foreach(IPerk perk in _perks)
                _currentValue = perk.Apply(_currentValue);

            EventValueChange?.Invoke(this);
        }
    }
}
