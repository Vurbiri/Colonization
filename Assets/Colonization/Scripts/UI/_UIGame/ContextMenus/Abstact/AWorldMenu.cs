//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\Abstact\AWorldMenu.cs
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public abstract class AWorldMenu : MonoBehaviour
    {
        protected GameObject _thisGO;
        protected Crossroad _currentCrossroad;

        protected readonly Signer<bool> _signer = new();

        protected virtual void Awake() => _thisGO = gameObject;

        protected virtual void OnEnable() => _signer.Invoke(true);

        public void Open() => _thisGO.SetActive(true);
        public void Close() => _thisGO.SetActive(false);

        protected virtual void OnDisable() => _signer.Invoke(false);

        protected virtual void OnClose()
        {
            _thisGO.SetActive(false);
        }
    }
}
