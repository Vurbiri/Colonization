using System;
using UnityEngine;

namespace Vurbiri.UI
{
    public abstract class AVButton : AVButtonBase
    {
        [SerializeField] protected UVAction _onClick = new();

        protected override void Start()
        {
            base.Start();
            _onClick.Init();
        }

        public Subscription AddListener(Action action) => _onClick.Add(action);
        public void RemoveListener(Action action) => _onClick.Remove(action);

        sealed protected internal override void Invoke() => _onClick.Invoke();
    }
}
