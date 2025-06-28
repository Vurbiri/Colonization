using UnityEngine;

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

        public bool Open => _isOpen;

        public void Init(MonoBehaviour parent, bool open = false)
        {
            _parent = parent;
            _canvasGroup.blocksRaycasts = _isOpen = open;
            _canvasGroup.alpha = open ? 1f : 0f;

            _waitLerp = new(0f, 1f, _speedSwitch, _canvasGroup.GetSetor<float>(nameof(_canvasGroup.alpha)));
        }

        public bool Switch()
        {
            _canvasGroup.blocksRaycasts = _isOpen = !_isOpen;
            StartCoroutine(_isOpen);

            return _isOpen;
        }

        public void Switch(bool open)
        {
            if (_isOpen == open)
                return;

            _canvasGroup.blocksRaycasts = _isOpen = open;
            StartCoroutine(open);
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
