//Assets\Vurbiri.UI\Runtime\UIElements\Utility\TransitionEffects\TransitionEffects.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    public abstract partial class TransitionEffect : ITransitionEffect
    {
        internal readonly MixColorTween _tween;
        protected Color _colorMarkOn, _colorMarkOff;

        public bool IsValid => _tween.IsValid;
        public bool Value => _tween.Check(_colorMarkOff);

        protected TransitionEffect() { }
        protected TransitionEffect(float duration, bool isOn, Graphic checkmark, Color colorMarkOn, Color colorMarkOff)
        {
            _colorMarkOn = colorMarkOn;
            _colorMarkOff = colorMarkOff;

            _tween = new(checkmark, duration);

            PlayInstant(isOn);
        }

        public bool SetGraphic(Graphic checkmarkA, Graphic checkmarkB) => _tween.SetTarget(checkmarkA);
        public void SetDuration(float duration) => _tween.markDuration = duration;
        public virtual void ColorsUpdate(Color colorOn, Color colorOff) { }

        public void Play(bool isOn)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { PlayInstant(isOn); return; }
#endif
            _tween.SetMarkColor(isOn ? _colorMarkOn : _colorMarkOff);
        }
        public void PlayInstant(bool isOn)
        {
            _tween.SetMarkColorInstant(isOn ? _colorMarkOn : _colorMarkOff);
        }

        public void Stop() => _tween.Stop();

        public void StateTransitionOn(Color targetColor, float duration)
        {
            _tween.SetStateColor(targetColor, duration);
        }
        public void StateTransitionOff(Color targetColor, float duration) { }
        public void StateTransitionClear() 
        {
            _tween.StateColorClear();
        }
    }
    //==================================================================================
    sealed public class EmptyEffect : ITransitionEffect
    {
        public EmptyEffect() { }

        public bool IsValid => false;
        public bool Value => throw new System.NotImplementedException();

        public bool SetGraphic(Graphic checkmarkA, Graphic checkmarkB) => false;
        public void SetDuration(float duration) { }
        public void ColorsUpdate(Color colorOn, Color colorOff) { }

        public void Play(bool isOn) { }
        public void PlayInstant(bool isOn) { }

        public void Stop() { }

        public void StateTransitionOn(Color targetColor, float duration) { }
        public void StateTransitionOff(Color targetColor, float duration) { }
        public void StateTransitionClear() { }
    }
    //==================================================================================
    public interface ITransitionEffect
    {
        public bool IsValid { get; }
        public bool Value { get; }

        public bool SetGraphic(Graphic checkmarkA, Graphic checkmarkB);
        public void SetDuration(float duration);
        public void ColorsUpdate(Color colorOn, Color colorOff);

        public void Play(bool isOn);
        public void PlayInstant(bool isOn);

        public void Stop();

        public void StateTransitionOn(Color targetColor, float duration);
        public void StateTransitionOff(Color targetColor, float duration);
        public void StateTransitionClear();
    }
}

