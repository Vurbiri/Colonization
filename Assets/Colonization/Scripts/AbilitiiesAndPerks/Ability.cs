using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class Ability
    {
        [SerializeField] private AbilityType _type;
        [SerializeField] private int _baseValue;
        [SerializeField] private AbilityType _parentType;

        private HashSet<Perk> _perks;
        private Ability _parentAbility;

        public AbilityType Type => _type;
        
        public int CurrentValue 
        { 
            get 
            {
                int currentValue = _baseValue;
                foreach (Perk perk in _perks)
                    currentValue = perk.Apply(currentValue);
                return currentValue; 
            } 
        }

        public Ability(AbilityType type, int baseValue, AbilityType parentType = AbilityType.None)
        {
            _type = type;
            _baseValue = baseValue;
            _parentType = parentType;
            _perks = new();
        }

        public bool TryAddPerk(Perk perk) => _perks.TryAdd(perk);
    }
}
