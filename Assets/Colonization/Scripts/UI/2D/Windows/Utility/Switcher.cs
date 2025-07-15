using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	[System.Serializable]
	public class Switcher
	{
        [SerializeField] private CanvasGroupSwitcher _canvasSwitcher;

        private bool _isOpen;
        private MonoBehaviour _parent;

        public readonly Subscription onOpen = new();
        public readonly Subscription onClose = new();

        public void Init(MonoBehaviour parent, bool open)
        {
            _parent = parent;
            _canvasSwitcher.Init(_isOpen = open);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Switch()
        {
            StartCoroutine(_isOpen = !_isOpen);
            
            if (_isOpen) 
                onOpen.Invoke();
            else
                onClose.Invoke();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Switch(bool open)
        {
            if (_isOpen != open)
                Switch();
        }

        private void StartCoroutine(bool open)
        {
            if (_canvasSwitcher.IsRunning)
                _parent.StopCoroutine(_canvasSwitcher);

            _parent.StartCoroutine(open ? _canvasSwitcher.Show() : _canvasSwitcher.Hide());
        }

#if UNITY_EDITOR
        public void OnValidate(MonoBehaviour parent)
        {
            _canvasSwitcher.OnValidate(parent);
        }
#endif
    }
}
