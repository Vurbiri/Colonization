//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\Abstact\AWorldMenu.cs
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public abstract class AWorldMenu : MonoBehaviour
    {
        protected GameObject _thisGO;
        protected Crossroad _currentCrossroad;

        protected readonly Subscriber<bool> _subscriber = new();

        protected virtual void Awake() => _thisGO = gameObject;

        protected virtual void OnEnable() => _subscriber.Invoke(true);

        public void Open() => _thisGO.SetActive(true);
        public void Close() => _thisGO.SetActive(false);

        protected virtual void OnDisable() => _subscriber.Invoke(false);

        protected virtual void OnClose()
        {
            _thisGO.SetActive(false);
        }
    }
}
