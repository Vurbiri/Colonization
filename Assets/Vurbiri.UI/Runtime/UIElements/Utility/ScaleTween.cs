//Assets\Vurbiri.UI\Runtime\UIElements\Utility\ScaleTween.cs
using System.Collections;
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
        private float _transitionTime, _currentTime;
        private float _progress = TRANSITION_END;
        private Coroutine _coroutine;
        private DrivenRectTransformTracker _tracker;

        private readonly bool _isValid;
        private bool _enable;

        public object Current => null;

        public ScaleTween()
        { 
            _isValid = _enable = false;
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
            if (duration < MIN_DUATION)
                Set(target);

            if (!(_enable & _isValid)) 
                return;

            StopCoroutine();

            _startScale = _targetTransform.localScale;

            if (_startScale != target)
            {
                _transitionTime = duration;
                _currentTime = duration * (TRANSITION_END - _progress);
                _targetScale = target;

                _coroutine = _target.StartCoroutine(this);
            }
            else
            {
                _progress = TRANSITION_END;
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
            if (_currentTime < _transitionTime)
            {
                _currentTime += Time.unscaledDeltaTime;
                _progress = _currentTime / _transitionTime;
                _targetTransform.localScale = Vector3.Lerp(_startScale, _targetScale, _progress);
                return true;
            }

            _progress = TRANSITION_END;
            _targetTransform.localScale = _targetScale;
            _coroutine = null;
            return false;
        }

        public void Reset() { }

        private void StopCoroutine()
        {
            if (_coroutine != null)
            {
                _target.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }
}
