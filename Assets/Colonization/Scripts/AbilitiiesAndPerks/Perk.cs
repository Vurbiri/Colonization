using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class Perk
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private TargetObjectPerk _target;
        [SerializeField] private AbilityType _ability;
        [SerializeField] private int _value;
        [SerializeField] private Chance _chance;
        [SerializeField] private Currencies _cost;

        public int Apply(int value) => value += _chance.Select(_value, 0);
    }
}
