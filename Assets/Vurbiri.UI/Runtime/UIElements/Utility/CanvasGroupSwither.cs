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

        private float _start, _end, _sign;
        private float _progress = 1f;

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
            Disable();
        }
        public void Set(bool show)
        {
            _canvasGroup.blocksRaycasts = show;
            _canvasGroup.alpha = _end = show ? 1f : 0f;
            _start = 1f - _end;
            _progress = 1f;
        }

        public void Disable()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0f;
            _start = 1f; _end = 0f;
            _progress = 1f;
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
        public void OnValidate(CanvasGroup canvasGroup)
        {
            _canvasGroup = canvasGroup;
            if (_speed < 0.1f) _speed = 0.1f;
        }
#endif
    }
}
