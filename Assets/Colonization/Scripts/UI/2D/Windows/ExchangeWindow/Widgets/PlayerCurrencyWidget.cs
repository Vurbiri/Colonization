using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	public class PlayerCurrencyWidget : ASelectOfCountWidget
    {
        [Space]
        [SerializeField] private Id<CurrencyId> _id; 
        
        private Unsubscription _unsubscriber;
        private readonly Subscription<int, int> _changeCount = new();

        public void Init(ACurrenciesReactive currencies, Action<int, int> action)
        {
            _unsubscriber = currencies.Get(_id).Subscribe(SetMax);
            _changeCount.Add(action);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void SetValue(int value)
        {
            base.SetValue(value);
            _changeCount.Invoke(_id.Value, value);
        }

        private void SetMax(int value)
        {
            _max = value;

            if(_count > _max)
            {
                _count = _max;
                SetValue(_count);
            }
        }

        private void OnDestroy() => _unsubscriber?.Unsubscribe();

#if UNITY_EDITOR

        [StartEditor] public Rect size;


        public void OnDrawGizmosSelected()
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(size.size.x, size.size.y, 0.01f));
        }
#endif
    }
}
