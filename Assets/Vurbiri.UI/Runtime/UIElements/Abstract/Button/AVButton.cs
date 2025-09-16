using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    public abstract class AVButton : AVButtonBase
    {
        [SerializeField] protected UniSubscription _onClick = new();

        protected override void Start()
        {
            base.Start();
            _onClick.Init();
        }

        public Unsubscription AddListener(Action action) => _onClick.Add(action);
        public void RemoveListener(Action action) => _onClick.Remove(action);

        sealed protected internal override void Invoke() => _onClick.Invoke();
    }
}
