//Assets\Vurbiri.UI\Runtime\Utility\TransitionEffects\SwitchEffect.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public class SwitchEffect : ITransitionEffect
    {
        private readonly OnOffEffect _markOn, _markOff;

        public bool IsValid => _markOn.IsValid & _markOff.IsValid;

        public bool Value => throw new System.NotImplementedException();

        public SwitchEffect(float duration, bool isOn, Graphic checkmarkOn, Graphic checkmarkOff)
        {
            _markOn = new(duration, isOn, checkmarkOn);
            _markOff = new(duration, !isOn, checkmarkOff);
        }

        public bool SetGraphic(Graphic checkmarkA, Graphic checkmarkB)
        {
            return _markOn.SetGraphic(checkmarkA, null) & _markOff.SetGraphic(checkmarkB, null);
        }

        public void SetDuration(float duration)
        {
            _markOn.SetDuration(duration);
            _markOff.SetDuration(duration);
        }

        public void ColorsUpdate(Color colorOn, Color colorOff) { }

        public void Play(bool isOn)
        {
            _markOn.Play(isOn);
            _markOff.Play(!isOn);
        }

        public void PlayInstant(bool isOn)
        {
            _markOn.PlayInstant(isOn);
            _markOff.PlayInstant(!isOn);
        }

        public void Stop()
        {
            _markOn.Stop();
            _markOff.Stop();
        }

        public void StateTransitionOn(Color targetColor, float duration)
        {
            _markOn.StateTransitionOn(targetColor, duration);
        }

        public void StateTransitionOff(Color targetColor, float duration)
        {
            _markOff.StateTransitionOn(targetColor, duration);
        }
    }
}
