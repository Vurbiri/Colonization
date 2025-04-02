//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\ColorTween.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.UI
{
    internal class ColorTween : IEnumerator
    {
        private Color _startColor;
        private Color _targetColor;
        private bool _fade;
        private float _duration;
        private float _speed;
        private float _progress;

        public CanvasRenderer canvasRenderer;

        public bool IsRunning => _fade && _progress < 0f;
        public object Current => null;

        public ColorTween(CanvasRenderer target)
        {
            canvasRenderer = target;
            _startColor = _targetColor = Color.white;
            SetTransition(false, 0f);
        }

        public void SetTransition(bool fade, float duration)
        {
            _fade = fade;
            if (fade)
            {
                _duration = duration; _progress = 0f; _speed = 1f / duration;
                return;
            }
            _duration = 0f; _progress = 1f; _speed = float.MaxValue;
        }

        public void Reset(Color targetColor)
        {
            _startColor = canvasRenderer.GetColor();
            _targetColor = targetColor;
            _progress = 0f;
        }
        public void Reset(Color targetColor, bool fade, float duration)
        {
            _startColor = canvasRenderer.GetColor();
            _targetColor = targetColor;
            SetTransition(fade, duration);
        }
        public void Reset() => _progress = 0f;

        public bool MoveNext()
        {
            if (_fade && _progress < 0f)
            {
                _progress += _speed * Time.unscaledDeltaTime;
                canvasRenderer.SetColor(Color.Lerp(_startColor, _targetColor, _progress));
                return true;
            }

            canvasRenderer.SetColor(_targetColor);
            _progress = 1f;
            return false;
        }


    }
}

