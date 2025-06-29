using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	[System.Serializable]
	public class Switcher
	{
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _speedSwitch = 8f;

        private bool _isOpen;
        private WaitSwitchFloat _waitLerp;
        private MonoBehaviour _parent;

        public readonly Subscription onOpen = new();
        public readonly Subscription onClose = new();

        public void Init(MonoBehaviour parent, bool open)
        {
            _parent = parent;
            _canvasGroup.blocksRaycasts = _isOpen = open;
            _canvasGroup.alpha = open ? 1f : 0f;

            _waitLerp = new(0f, 1f, _speedSwitch, _canvasGroup.GetSetor<float>(nameof(_canvasGroup.alpha)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Switch()
        {
            _canvasGroup.blocksRaycasts = _isOpen = !_isOpen;
            StartCoroutine(_isOpen);
            
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
            if (_waitLerp.IsRunning)
                _parent.StopCoroutine(_waitLerp);

            _parent.StartCoroutine(open ? _waitLerp.Forward(_canvasGroup.alpha) : _waitLerp.Backward(_canvasGroup.alpha));
        }

#if UNITY_EDITOR
        public void OnValidate(MonoBehaviour parent)
        {
            parent.SetComponent(ref _canvasGroup);
        }
#endif
    }
}
