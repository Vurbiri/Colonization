//Assets\Vurbiri.UI\Runtime\Toggle\VToggleTransitionEffects.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {

        //==================================================================================
        sealed private class OnOffEffect : TransitionEffectSetAlpha
        {
            internal static bool Validate(VToggle parent) => parent._checkmarkOn != null;

            public override bool IsValid => Validate(_parent);

            public OnOffEffect(VToggle parent) : base(parent)
            {
                _parent._checkmarkOn.enabled = true;
                PlayInstant();
            }

            public override void Play()
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) { PlayInstant(); return; }
#endif
                if (_instant) PlayInstant();
                else PlayDuration(_parent._checkmarkOn);
            }

            public override void PlayInstant()
            {
                _parent._checkmarkOn.color = _colorOn.SetAlpha(_parent._isOn ? 1f : 0f);
            }

            private void PlayDuration(Graphic graphic)
            {
                graphic.StartCoroutine(FadeAlpha_Cn(graphic, _parent._isOn ? 1f : 0f));
            }
        }
        //==================================================================================
        sealed private class SwitchEffect : TransitionEffect
        {
            internal static bool Validate(VToggle parent) => parent._checkmarkOn != null & parent._checkmarkOff != null;

            private float _duration;

            public override bool IsValid => Validate(_parent);

            public SwitchEffect(VToggle parent) : base(parent)
            {
                PlayInstant();
            }

            public override void UpdateTransition()
            {
                _duration = _parent._markTransition == ToggleTransition.Fade ? _parent._transitionDuration : 0f;
            }

            public override void Play()
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) { PlayInstant(); return; }
#endif
                float on = 0f, off = CurrentAlfa;
                if (_parent._isOn)
                { on = off; off = 0f; }

                _parent._checkmarkOff.CrossFadeAlpha(off, _duration, true);
                _parent._checkmarkOn.CrossFadeAlpha(on, _duration, true);

            }

            public override void PlayInstant()
            {
                float on = 0f, off = CurrentAlfa;
                if (_parent._isOn)
                { on = off; off = 0f; }

                _parent._checkmarkOn.canvasRenderer.SetAlpha(on);
                _parent._checkmarkOff.canvasRenderer.SetAlpha(off);
            }
        }
        //==================================================================================
        sealed private class ColorEffect : TransitionEffect
        {
            internal static bool Validate(VToggle parent)
            {
                if(parent._checkmarkOn == null) return false;

                return parent._markTransition == ToggleTransition.Instant | !Mathf.Approximately(parent._transitionDuration, 0);
            }

            private Color _target;
            private Coroutine _coroutine;

            public override bool IsValid => Validate(_parent);
            public override bool Value => _parent._checkmarkOn.color == _parent._colorOn;

            public ColorEffect(VToggle parent) : base(parent)
            {
                _parent._checkmarkOn.enabled = true;
                PlayInstant();
            }

            public override void Play()
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) { PlayInstant(); return; }
#endif

                if (_instant) PlayInstant();
                else PlayDuration();
            }

            public override void PlayInstant()
            {
                _parent._checkmarkOn.color = _parent._isOn ? _parent._colorOn : _parent._colorOff;
            }

            private void PlayDuration()
            {
                _target = _parent._isOn ? _parent._colorOn : _parent._colorOff;
                _coroutine ??= _parent.StartCoroutine(PlayDuration_Cn());
            }

            private IEnumerator PlayDuration_Cn()
            {
                Graphic graphic = _parent._checkmarkOn;
                float progress = 0f;

                while (progress <= 1f)
                {
                    yield return null;
                    progress += _speed * Time.unscaledDeltaTime;
                    graphic.color = Color.Lerp(graphic.color, _target, progress);
                }

                graphic.color = _target;
                _coroutine = null;
            }
        }
        //==================================================================================
        private abstract class TransitionEffect
        {
            protected VToggle _parent;
            protected readonly Color _colorOn;
            protected bool _instant;
            protected float _speed;

            public abstract bool IsValid { get; }
            public virtual bool Value => !Mathf.Approximately(_parent._checkmarkOn.canvasRenderer.GetAlpha(), 0);

            protected TransitionEffect() { }
            protected TransitionEffect(VToggle parent)
            {
                _parent = parent;
                _colorOn = _parent._checkmarkOn.color;
                UpdateTransition();
            }

            public virtual void UpdateTransition()
            {
                _instant = _parent._markTransition == ToggleTransition.Instant;
                _speed = 1f / _parent._transitionDuration;
            }

            public abstract void Play();
            public abstract void PlayInstant();

            protected float CurrentAlfa => 1f;

            public static TransitionEffect Create(VToggle parent)
            {
                if (parent._checkmarkOn != null) parent._checkmarkOn.enabled = false;
                if (parent._checkmarkOff != null) parent._checkmarkOff.enabled = false;

                if (parent._transitionType == TransitionType.OnOffCheckmark && OnOffEffect.Validate(parent))
                    return new OnOffEffect(parent);

                if (parent._transitionType == TransitionType.SwitchCheckmark && SwitchEffect.Validate(parent))
                    return new SwitchEffect(parent);

                if (parent._transitionType == TransitionType.ColorCheckmark && ColorEffect.Validate(parent))
                    return new ColorEffect(parent);

                return new Empty();
            }
        }
        //==================================================================================
        private abstract class TransitionEffectSetAlpha : TransitionEffect
        {
            protected TransitionEffectSetAlpha(VToggle parent) : base(parent) { }

            protected IEnumerator FadeAlpha_Cn(Graphic graphic, float alpha)
            {
                Color color = graphic.color;
                float current = color.a;
                float progress = 0f;

                while (progress <= 1f)
                {
                    yield return null;
                    progress += _speed * Time.unscaledDeltaTime;
                    current = Mathf.Lerp(current, alpha, progress);
                    graphic.color = color.SetAlpha(current);
                }
                graphic.color = color.SetAlpha(alpha);
            }
        }
        //==================================================================================
        sealed private class Empty : TransitionEffect
        {
            public Empty() { }

            public override bool IsValid => false;
            public override bool Value => throw new System.NotImplementedException();

            public override void Play() { }

            public override void PlayInstant() { }

            public override void UpdateTransition() { }
        }
    }
}

