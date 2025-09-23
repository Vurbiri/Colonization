using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
	public abstract class AVButton<T> : AVButtonBase
    {
        [SerializeField] protected T _value;
        [SerializeField] protected UniSubscription<T> _onClick = new();

        public Event<T> OnClick => _onClick;

        protected override void Start()
        {
            base.Start();
            _onClick.Init(_value);
        }

        public Unsubscription AddListener(Action<T> action) => _onClick.Add(action);
        public void RemoveListener(Action<T> action) => _onClick.Remove(action);

        sealed protected internal override void Invoke() => _onClick.Invoke(_value);
    }
}