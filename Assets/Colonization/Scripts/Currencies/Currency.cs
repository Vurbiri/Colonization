using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class Currency : AReactive<int>
    {
        [SerializeField] private int _value;
        [SerializeField] private bool _isAmount;

        protected override void Callback(Action<int> action) => action(_value);
    }
}
