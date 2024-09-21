using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class Perk : IPerk, IPerkUI
    {
        [SerializeField] private int _id;
        [SerializeField] private int _level;
        [SerializeField] private string _keyName;
        [SerializeField] private string _keyDescription;
        [SerializeField] private TargetObjectPerk _target;
        [SerializeField] private AbilityType _ability;
        [SerializeField] private int _value;
        [SerializeField] private Chance _chance;
        [SerializeField] private Currencies _cost;

        public int Id => _id;
        public int Level => _level;
        public string KeyName => _keyName;
        public string KeyDescription => _keyDescription;
        public TargetObjectPerk TargetObject => _target;
        public AbilityType TargetAbility => _ability;
        public Currencies Cost => _cost;
        public bool IsPermanent => _chance == 100;
       
        public int Apply(int value) => value += _chance == 100 ? _value : _chance.Select(_value, 0);
    }
}
