using System;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public abstract class ACrossroadMenu : MonoBehaviour
    {
        protected GameObject _thisGO;
        protected Crossroad _currentCrossroad;

        public event Action<bool> EventEnabled;

        protected virtual void Awake() => _thisGO = gameObject;

        protected virtual void OnEnable() => EventEnabled?.Invoke(true);

        public abstract void Open(Crossroad crossroad);

        public void Open() => _thisGO.SetActive(true);
        public void Close() => _thisGO.SetActive(false);

        protected virtual void OnDisable() => EventEnabled?.Invoke(false);

        protected virtual void OnDestroy() => EventEnabled = null;
    }
}
