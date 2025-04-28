//Assets\Vurbiri.UI\Runtime\UIElements\Utility\ScaleTween.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.UI
{
    public class ScaleTween : IEnumerator
    {
        private const float TRANSITION_END = 1f;
        
        private readonly MonoBehaviour _mono;
        private readonly RectTransform _targetTransform;

        private Vector3 _startScale;
        private Vector3 _targetScale;
        private float _transitionTime, _currentTime;
        private float _progress = TRANSITION_END;
        private Coroutine _coroutine;
        private DrivenRectTransformTracker _tracker;

        public readonly bool isValid;

        public Vector3 CurrentColor => _targetTransform.localScale;
        public float Progress => _progress;
        public bool IsRunning => _coroutine != null;
        public object Current => null;

        public ScaleTween(RectTransform targetTransform, MonoBehaviour mono)
        {
            _targetTransform = targetTransform;
            _mono = mono;
            isValid = _targetTransform != null && _mono != null;
        }

        public ScaleTween()
        { 
            isValid = false;
        }

        public void Set(Vector3 target)
        {
            Stop();

            _progress = TRANSITION_END;
            _tracker.Add(_mono, _targetTransform, DrivenTransformProperties.Scale);

            _targetTransform.localScale = target;
        }

        public void Set(Vector3 target, float duration)
        {
            Stop();

            _startScale = _targetTransform.localScale;

            if (_startScale != target)
            {
                _transitionTime = duration;
                _currentTime = duration * (TRANSITION_END - _progress);

                _targetScale = target;

                _tracker.Add(_mono, _targetTransform, DrivenTransformProperties.Scale);

                _coroutine = _mono.StartCoroutine(this);
            }
            else
            {
                _progress = TRANSITION_END;
            }
        }

        public void TrackerClear() => _tracker.Clear();

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

        private void Stop()
        {
            _tracker.Clear();
            
            if (_coroutine != null)
            {
                _mono.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }
}
