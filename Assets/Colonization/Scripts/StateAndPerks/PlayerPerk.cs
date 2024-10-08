using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class PlayerPerk : IPerk<PlayerStateId>, IPerkUI
    {
        [SerializeField] private int _id;
        [SerializeField] private int _level;
        [SerializeField] private string _keyName;
        [SerializeField] private string _keyDescription;
        [SerializeField] private Id<TargetOfPerkId> _target;
        [SerializeField] private Id<PlayerStateId> _ability;
        [SerializeField] private int _value;
        [SerializeField] private Chance _chance = 100;
        [SerializeField] private CurrenciesLite _cost;

        public int Id => _id;
        public int Level => _level;
        public string KeyName => _keyName;
        public string KeyDescription => _keyDescription;
        public Id<TargetOfPerkId> TargetObject => _target;
        public Id<PlayerStateId> TargetAbility => _ability;
        public CurrenciesLite Cost => _cost;
        public bool IsPermanent => _chance == 100;

        public int Apply(int value) => value + (_chance == 100 ? _value : _chance.Select(_value, 0));
    }
}
