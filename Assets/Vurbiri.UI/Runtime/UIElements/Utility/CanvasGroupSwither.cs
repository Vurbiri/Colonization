using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.UI
{
	[System.Serializable]
	public class CanvasGroupSwitcher : IEnumerator
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _speed;

        private float _start = 0f, _end = 1f, _sign = 1f;
        private float _progress;

        public object Current => null;
        public bool IsRunning => _progress < 1f;
        public float Speed { get => _speed; set => _speed = value; }
        public float Alpha { get => _canvasGroup.alpha; set => _canvasGroup.alpha = value; }
        public bool BlocksRaycasts { get => _canvasGroup.blocksRaycasts; set => _canvasGroup.blocksRaycasts = value; }
        public CanvasGroup CanvasGroup => _canvasGroup;

        public CanvasGroupSwitcher(float speed, CanvasGroup canvasGroup)
        {
            _speed = speed;
            _canvasGroup = canvasGroup;
            Init();
        }

        public void Init()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
        public void Init(bool show)
        {
            _canvasGroup.blocksRaycasts = show;
            _canvasGroup.alpha = _start = show ? 1f : 0f;
            _end = 1f - _start; _sign = _end - _start;
        }

        public bool MoveNext()
        {
            _progress += Time.unscaledDeltaTime * _speed;
            if (_progress < 1f)
            {
                _canvasGroup.alpha = _start + _sign * _progress;
                return true;
            }

            _canvasGroup.alpha = _end;
            _canvasGroup.blocksRaycasts = _end > 0.5f;
            return false;
        }

        public IEnumerator Show() => Set(0f, 1f);
        public IEnumerator Hide() => Set(1f, 0f);
        public IEnumerator Switch() => Set(_end, _start);

        public void Reset()
        {
            _progress = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerator Set(float start, float end)
        {
            _start = start; _end = end; _sign = end - start;
            _progress = (_canvasGroup.alpha - _start) / _sign;

            _canvasGroup.blocksRaycasts = false;
            return this;
        }

#if UNITY_EDITOR
        public void OnValidate(Component parent)
        {
            parent.SetComponent(ref _canvasGroup);
            if (_speed < 0.1f) _speed = 0.1f;
        }
#endif
    }
}
