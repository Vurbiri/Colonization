using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
    [System.Serializable]
    public class CanvasGroupSwitcher : IEnumerator
    {
        public const float MIN_SPEED = 0.05f;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _speed;

        private float _start, _end, _sign;
        private float _progress = 1f;

        public object Current => null;

        public bool Valid { [Impl(256)] get => _canvasGroup != null; }
        public float Alpha { [Impl(256)] get => _canvasGroup.alpha; }
        public bool BlocksRaycasts { [Impl(256)] get => _canvasGroup.blocksRaycasts; }
        public bool IsRunning{ [Impl(256)] get => _progress < 1f; }
        public bool IsShow{ [Impl(256)] get => _end > 0.95f; }
        public float Speed
        {
            [Impl(256)] get => _speed;
            set
            {
                if (value < MIN_SPEED) value = MIN_SPEED;
                _speed = value;
            }
        }

        public CanvasGroupSwitcher(CanvasGroup canvasGroup, float speed)
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
        public void Enable()
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            _start = 0f; _end = 1f;
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
            _canvasGroup.blocksRaycasts = IsShow;
            return false;
        }

        public IEnumerator Show() => Set(0f, 1f);
        public IEnumerator Hide() => Set(1f, 0f);
        public IEnumerator Switch() => Set(_end, _start);

        [Impl(256)]
        public void Reset()
        {
            _progress = 0f;
        }

        [Impl(256)]
        private IEnumerator Set(float start, float end)
        {
            _start = start; _end = end; _sign = end - start;
            _progress = (_canvasGroup.alpha - _start) / _sign;

            _canvasGroup.blocksRaycasts = false;
            return this;
        }

#if UNITY_EDITOR
        public void OnValidate(MonoBehaviour parent)
        {
            parent.SetChildren(ref _canvasGroup);
            if (_speed < MIN_SPEED) _speed = MIN_SPEED;
        }
        public void OnValidate(MonoBehaviour parent, float minSpeed)
        {
            parent.SetChildren(ref _canvasGroup);
            if (_speed < minSpeed) _speed = minSpeed;
        }
        public void OnValidate(MonoBehaviour parent, string name)
        {
            parent.SetChildren(ref _canvasGroup, name);
            if (_speed < MIN_SPEED) _speed = MIN_SPEED;
        }
        public void OnValidate(MonoBehaviour parent, string name, float minSpeed)
        {
            parent.SetChildren(ref _canvasGroup, name);
            if (_speed < minSpeed) _speed = minSpeed;
        }
#endif
    }
}
