//Assets\Vurbiri.UI\Runtime\Utility\ColorTween.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    public class ColorTween : IEnumerator
    {
        private Graphic _target;
        private CanvasRenderer _canvasRenderer;
        private Color _startColor, _targetColor;
        private bool _fade;
        private float _duration, _speed;
        private float _progress = 1f;
        private Coroutine _coroutine;

        public bool IsValid => Validate(_target, _fade, _duration);
        public Color CurrentColor => _canvasRenderer.GetColor();
        public bool IsRunning => _coroutine != null;
        public object Current => null;

        public ColorTween(Graphic target)
        {
            _target = target;
            _canvasRenderer = target.canvasRenderer;
            SetTransition(false, 0f);
        }
        public ColorTween(Graphic target, bool fade, float duration)
        {
            _target = target;
            _canvasRenderer = target.canvasRenderer;
            SetTransition(fade, duration);
        }

        public void SetColor(Color color) => _canvasRenderer.SetColor(color);

        public void SetTransition(bool fade, float duration)
        {
            _fade = fade;
            if (fade) { _duration = duration; _speed = 1f / duration; }
            else      { _duration = 0f;       _speed = float.MaxValue; }
        }
        public bool SetTarget(Graphic target)
        {
            if (IsRunning) { _target.StopCoroutine(_coroutine); Reset(); }
            
            _target = target;
            if (target == null) _canvasRenderer = null;
            else _canvasRenderer = target.canvasRenderer;

            return _canvasRenderer;
        }

        public void Start(Color targetColor)
        {
            _startColor = _canvasRenderer.GetColor();
            _targetColor = targetColor;
            _progress = StopAndGetProgress();
            _coroutine = _target.StartCoroutine(this);
        }
        public void Start(Color targetColor, bool fade, float duration)
        {
            SetTransition(fade, duration);
            Start(targetColor);
        }

        public bool MoveNext()
        {
            if (_fade & _progress < 1f)
            {
                _progress += _speed * Time.unscaledDeltaTime;
                _canvasRenderer.SetColor(Color.Lerp(_startColor, _targetColor, _progress));
                return true;
            }

            _canvasRenderer.SetColor(_targetColor);
            _progress = 1f;
            _coroutine = null;
            return false;
        }

        public void Reset() => _progress = 1f;

        public static bool Validate(Graphic graphic, bool fade, float duration)
        {
            return graphic != null & (!fade || !Mathf.Approximately(duration, 0f));
        }

        private float StopAndGetProgress()
        {
            if (_coroutine == null) return 0f;

            _target.StopCoroutine(_coroutine);
            return Mathf.Clamp01(1f - _progress);
        }
    }
}

