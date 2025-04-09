//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\SwitchEffect.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        sealed private class SwitchEffect : ITransitionEffect
        {
            internal static bool Validate(VToggle parent) => parent._checkmarkOn != null & parent._checkmarkOff != null;

            private readonly OnOffEffect _markOn, _markOff;
 
            public bool IsValid => _markOn.IsValid & _markOff.IsValid;

            public bool Value => throw new System.NotImplementedException();

            public SwitchEffect(VToggle parent)
            {
                _markOn = new(parent._fadeDuration, parent._isOn, parent._checkmarkOn);
                _markOff = new(parent._fadeDuration, !parent._isOn, parent._checkmarkOff);
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

            public void ColorsUpdate() { }

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
}
