//Assets\Vurbiri.UI\Runtime\UIElements\Utility\ColorTween.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    public class ColorTween : IEnumerator
    {
        private Graphic _target;
        private CanvasRenderer _canvasRenderer;
        private Color _startColor;
        private Color _targetColor;
        private float _duration;
        private float _progress;
        private Coroutine _coroutine;

        public bool IsValid => _canvasRenderer != null;
        public Color CurrentColor => _canvasRenderer.GetColor();
        public float CurrentDuration => _progress;
        public bool IsRunning => _coroutine != null;
        public object Current => null;

        public ColorTween(Graphic target)
        {
            _target = target;
            _canvasRenderer = target.canvasRenderer;
        }

        public void Set(Color target)
        {
            if (_coroutine != null) 
            { 
                _target.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _canvasRenderer.SetColor(target);
        }

        public void Set(Color target, float duration)
        {
            if (_coroutine != null)
            {
                _target.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _startColor = _canvasRenderer.GetColor();
            _targetColor = target;
            _duration = duration;
            _progress = 0f;

            if (_startColor != target)
                _coroutine = _target.StartCoroutine(this);
        }

        public bool MoveNext()
        {
            if (_progress < _duration)
            {
                _progress += Time.unscaledDeltaTime;
                _canvasRenderer.SetColor(Color.Lerp(_startColor, _targetColor, _progress / _duration));
                return true;
            }

            _canvasRenderer.SetColor(_targetColor);
            _progress = _duration;
            _coroutine = null;
            return false;
        }

        public void Reset() { }
    }
}

