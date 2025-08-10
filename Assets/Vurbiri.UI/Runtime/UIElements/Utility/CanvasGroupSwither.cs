using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.UI
{
	[System.Serializable]
	public class CanvasGroupSwitcher : IEnumerator
    {
        public const float MIN_SPEED = 0.05f;

        public CanvasGroup canvasGroup;
        public float speed;

        private float _start, _end, _sign;
        private float _progress = 1f;

        public object Current => null;
        public bool IsRunning => _progress < 1f;
        public float Alpha { get => canvasGroup.alpha; set => canvasGroup.alpha = value; }
        public bool BlocksRaycasts { get => canvasGroup.blocksRaycasts; set => canvasGroup.blocksRaycasts = value; }
        public bool IsShow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _end > 0.95f; }
        }

        public CanvasGroupSwitcher(CanvasGroup canvasGroup, float speed)
        {
            this.speed = speed;
            this.canvasGroup = canvasGroup;
            Disable();
        }

        public void Set(bool show)
        {
            canvasGroup.blocksRaycasts = show;
            canvasGroup.alpha = _end = show ? 1f : 0f;
            _start = 1f - _end;
            _progress = 1f;
        }
        public void Enable()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            _start = 0f; _end = 1f;
            _progress = 1f;
        }
        public void Disable()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
            _start = 1f; _end = 0f;
            _progress = 1f;
        }
        
        public bool MoveNext()
        {
            _progress += Time.unscaledDeltaTime * speed;
            if (_progress < 1f)
            {
                canvasGroup.alpha = _start + _sign * _progress;
                return true;
            }

            canvasGroup.alpha = _end;
            canvasGroup.blocksRaycasts = IsShow;
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
            _progress = (canvasGroup.alpha - _start) / _sign;

            canvasGroup.blocksRaycasts = false;
            return this;
        }

#if UNITY_EDITOR
        public void OnValidate(MonoBehaviour parent)
        {
            parent.SetChildren(ref canvasGroup);
            if (speed < MIN_SPEED) speed = MIN_SPEED;
        }
        public void OnValidate(MonoBehaviour parent, float minSpeed)
        {
            parent.SetChildren(ref canvasGroup);
            if (speed < minSpeed) speed = minSpeed;
        }
        public void OnValidate(MonoBehaviour parent, string name)
        {
            parent.SetChildren(ref canvasGroup, name);
            if (speed < MIN_SPEED) speed = MIN_SPEED;
        }
#endif
    }
}
