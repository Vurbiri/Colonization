//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\TransitionEffects.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        private abstract class TransitionEffect : ITransitionEffect
        {
            protected readonly VToggle _parent;
            protected readonly ColorTween _tweenMark, _tweenState;

            protected Color _colorMarkOn, _colorMarkOff;
            protected Color _targetColorMark, _targetColorState;

            public bool IsValid => _tweenMark.IsValid;
            public bool Value => _tweenMark.CurrentColor != _colorMarkOff * _targetColorState;

            protected Color TargetColor => _targetColorMark * _targetColorState;

            protected TransitionEffect() { }
            protected TransitionEffect(VToggle parent, bool isOn, Graphic checkmark, Color colorMarkOn, Color colorMarkOff)
            {
                _parent = parent;

                _colorMarkOn = colorMarkOn;
                _colorMarkOff = colorMarkOff;

                _targetColorMark = isOn ? colorMarkOn : colorMarkOff;
                _targetColorState = Color.white;

                _tweenMark = new(checkmark, parent._isFade, parent._fadeDuration);
                _tweenState = new(checkmark);

                PlayInstant(isOn);
            }

            public bool SetGraphic(Graphic checkmarkA, Graphic checkmarkB) => _tweenMark.SetTarget(checkmarkA) & _tweenState.SetTarget(checkmarkA);
            public void TransitionUpdate() => _tweenMark.SetTransition(_parent._isFade, _parent._fadeDuration);
            public virtual void ColorsUpdate() { }

            public void Play(bool isOn)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) { PlayInstant(isOn); return; }
#endif
                _targetColorMark = isOn ? _colorMarkOn : _colorMarkOff;
                _tweenMark.Start(TargetColor);
            }
            public void PlayInstant(bool isOn)
            {
                _targetColorMark = _parent._isOn ? _colorMarkOn : _colorMarkOff;
                _tweenMark.SetColor(TargetColor);
            }

            public void StateTransitionOn(Color targetColor, float duration, bool instant) 
            {
                _targetColorState = targetColor;
                _tweenState.Start(TargetColor, !instant, duration);
            }
            public void StateTransitionOff(Color targetColor, float duration, bool instant) { }
        }
        //==================================================================================
        sealed private class EmptyEffect : ITransitionEffect
        {
            public EmptyEffect() { }

            public bool IsValid => false;
            public bool Value => throw new System.NotImplementedException();

            public bool SetGraphic(Graphic checkmarkA, Graphic checkmarkB) => false;
            public void TransitionUpdate() { }
            public void ColorsUpdate() { }

            public void Play(bool isOn) { }
            public void PlayInstant(bool isOn) { }

            public void StateTransitionOn(Color targetColor, float duration, bool instant) { }
            public void StateTransitionOff(Color targetColor, float duration, bool instant) { }
        }
        //==================================================================================
        private interface ITransitionEffect
        {
            public bool IsValid { get; }
            public bool Value { get; }

            public bool SetGraphic(Graphic checkmarkA, Graphic checkmarkB);
            public void TransitionUpdate();
            public void ColorsUpdate();

            public void Play(bool isOn);
            public void PlayInstant(bool isOn);

            public void StateTransitionOn(Color targetColor, float duration, bool instant);
            public void StateTransitionOff(Color targetColor, float duration, bool instant);
        }
    }
}

