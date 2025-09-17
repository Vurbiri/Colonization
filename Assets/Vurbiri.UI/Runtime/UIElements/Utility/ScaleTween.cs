using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.UI
{
    public class ScaleTween : IEnumerator
    {
        private const float MIN_DUATION = 0.03f, TRANSITION_END = 1f;
        
        private readonly MonoBehaviour _target;
        private readonly RectTransform _targetTransform;

        private Vector3 _startScale = Vector3.one;
        private Vector3 _targetScale = Vector3.one;
        private float _speed;
        private float _progress = TRANSITION_END;
        private bool _isRunning;
        private Coroutine _coroutine;
        private DrivenRectTransformTracker _tracker;

        private readonly bool _isValid;
        private bool _enable;

        public object Current => null;

        public bool IsRunning => _isRunning;

        public ScaleTween()
        { 
            _isValid = _enable = _isRunning = false;
        }
        private ScaleTween(MonoBehaviour target, RectTransform targetTransform)
        {
            _targetTransform = targetTransform;
            _target = target;
            _isValid = _targetTransform != null && _target != null;
        }

        public ScaleTween ReCreate(MonoBehaviour target, RectTransform targetTransform)
        {
            Disable();
            return new(target, targetTransform);
        }
        public ScaleTween ReCreate(MonoBehaviour target, RectTransform targetTransform, bool isActive)
        {
            ScaleTween scaleTween = ReCreate(target, targetTransform);
            scaleTween.SetActive(isActive);
            return scaleTween;
        }

        public void Set(Vector3 target)
        {
            if (_enable & _isValid)
            {
                StopCoroutine();

                _progress = TRANSITION_END;
                _targetTransform.localScale = target;
            }
        }

        public void Set(Vector3 target, float duration)
        {
            duration *= _progress;
            if (duration < MIN_DUATION)
                Set(target);

            if (_enable & _isValid)
            {
                StopCoroutine();

                _startScale = _targetTransform.localScale;

                if (_startScale != target)
                {
                    _targetScale = target;
                    _speed = 1f / duration;
                    _progress = 0f;
     
                    _coroutine = _target.StartCoroutine(this);
                }
                else
                {
                    _progress = TRANSITION_END;
                }
            }
        }

        public void Enable()
        {
            if (!_enable & _isValid)
                _tracker.Add(_target, _targetTransform, DrivenTransformProperties.Scale);

            _enable = true;
        }

        public void Disable()
        {
            Set(_targetScale);

            _tracker.Clear();
            _enable = false;
        }

        public void SetActive(bool active)
        {
            if (active) 
                Enable();
            else 
                Disable();
        }

        public bool MoveNext()
        {
            if (_isRunning = (_progress += _speed * Time.unscaledDeltaTime) < TRANSITION_END)
            {
                _targetTransform.localScale = Lerp(_startScale, _targetScale, _progress);
            }
            else
            {
                _progress = TRANSITION_END;
                _targetTransform.localScale = _targetScale;
            }

            return _isRunning;

            // Local
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Vector3 Lerp(Vector3 a, Vector3 b, float t)
            {
                return new (a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
            }
        }

        public void Reset() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StopCoroutine()
        {
            if (_isRunning)
            {
                _target.StopCoroutine(_coroutine);
                _isRunning = false;
            }
        }
    }
}
