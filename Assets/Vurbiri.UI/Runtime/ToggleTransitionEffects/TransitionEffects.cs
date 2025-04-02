//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\TransitionEffects.cs
using UnityEngine;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        private abstract class TransitionEffect : ITransitionEffect
        {
            protected readonly VToggle _parent;
            protected readonly ColorTween _tweenMarkA;
            protected readonly ColorTween _tweenStateA;

            protected Color _colorMarkOn, _colorMarkOff;
            protected Color _targetColorMarkA, _targetColorStateA;

            public abstract bool IsValid { get; }
            public virtual bool Value => !Mathf.Approximately(_parent._checkmarkOn.canvasRenderer.GetAlpha(), 0);

            protected Color TargetColorA => _targetColorMarkA * _targetColorStateA;

            protected TransitionEffect() { }
            protected TransitionEffect(VToggle parent)
            {
                _parent = parent;
                
                _targetColorStateA = _parent.CurrentColor;
                _tweenMarkA = new(_parent._checkmarkOn.canvasRenderer);
                _tweenStateA = new(_parent._checkmarkOn.canvasRenderer);
                TransitionUpdate();
            }

            public virtual void TransitionUpdate()
            {
                _tweenMarkA.SetTransition(_parent._markTransition == ToggleTransition.Fade, _parent._transitionDuration);
            }

            public bool GraphicUpdate()
            {
                if (_parent._checkmarkOn == null) return false;
                return _tweenMarkA.canvasRenderer = _tweenStateA.canvasRenderer  = _parent._checkmarkOn.canvasRenderer;
            }
            public virtual void ColorsUpdate() { }

            public void Play()
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) { PlayInstant(); return; }
#endif
                PlayDuration();
            }
            public virtual void PlayInstant()
            {
                _targetColorMarkA = _parent._isOn ? _colorMarkOn : _colorMarkOff;
                _tweenMarkA.canvasRenderer.SetColor(TargetColorA);
            }
            public virtual void PlayDuration()
            {
                _targetColorMarkA = _parent._isOn ? _colorMarkOn : _colorMarkOff;
                _tweenMarkA.Reset(TargetColorA);
                _parent._checkmarkOn.StartCoroutine(_tweenMarkA);
            }

            public virtual void StateTransitionOn(Color targetColor, float duration, bool instant) 
            {
                _targetColorStateA = targetColor;
                _tweenStateA.Reset(TargetColorA, !instant, duration);
                _parent._checkmarkOn.StartCoroutine(_tweenStateA);

            }
            public virtual void StateTransitionOff(Color targetColor, float duration, bool instant) { }


            public static ITransitionEffect Create(VToggle parent)
            {
                if (parent._checkmarkOn != null) parent._checkmarkOn.canvasRenderer.SetAlpha(0f);
                if (parent._checkmarkOff != null) parent._checkmarkOff.canvasRenderer.SetAlpha(0f);

                if (parent._transitionType == TransitionType.OnOffCheckmark && OnOffEffect.Validate(parent))
                    return new OnOffEffect(parent);

                /*if (parent._transitionType == TransitionType.SwitchCheckmark && SwitchEffect.Validate(parent))
                    return new SwitchEffect(parent);*/

                if (parent._transitionType == TransitionType.ColorCheckmark && ColorEffect.Validate(parent))
                    return new ColorEffect(parent);

                return new EmptyEffect();
            }

        }
        //==================================================================================
        sealed private class EmptyEffect : ITransitionEffect
        {
            public EmptyEffect() { }

            public bool IsValid => false;
            public bool Value => throw new System.NotImplementedException();

            public void TransitionUpdate() { }
            public bool GraphicUpdate() => false;
            public void ColorsUpdate() { }

            public void PlayInstant() { }
            public void PlayDuration() { }

            public void Play() { }

            public void StateTransitionOn(Color targetColor, float duration, bool instant) { }
            public void StateTransitionOff(Color targetColor, float duration, bool instant) { }
        }

        private interface ITransitionEffect
        {
            public bool IsValid { get; }
            public bool Value { get; }

            public void TransitionUpdate();
            public bool GraphicUpdate();
            public void ColorsUpdate();
            

            public void Play();
            public void PlayInstant();

            public void StateTransitionOn(Color targetColor, float duration, bool instant);
            public void StateTransitionOff(Color targetColor, float duration, bool instant);
        }
    }
}

