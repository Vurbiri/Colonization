using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	[System.Serializable]
	public class Switcher
	{
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private WaitSwitchFloat _waitSwitch;

        private bool _isOpen;
        private MonoBehaviour _parent;

        public readonly Subscription onOpen = new();
        public readonly Subscription onClose = new();

        public void Init(MonoBehaviour parent, bool open)
        {
            _parent = parent;
            _canvasGroup.blocksRaycasts = _isOpen = open;
            _canvasGroup.alpha = open ? 1f : 0f;

            _waitSwitch.Init();
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
            if (_waitSwitch.IsRunning)
                _parent.StopCoroutine(_waitSwitch);

            _parent.StartCoroutine(open ? _waitSwitch.Forward(_canvasGroup.alpha) : _waitSwitch.Backward(_canvasGroup.alpha));
        }

#if UNITY_EDITOR
        public void OnValidate(MonoBehaviour parent)
        {
            parent.SetComponent(ref _canvasGroup);

            if (!_waitSwitch.IsValid_Editor)
                _waitSwitch.OnValidate(0f, 1f, _canvasGroup.GetSetor<float>(nameof(_canvasGroup.alpha)));
        }
#endif
    }
}
