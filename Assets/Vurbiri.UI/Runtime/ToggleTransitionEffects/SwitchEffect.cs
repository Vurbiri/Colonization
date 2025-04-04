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

            private readonly VToggle _parent;
            private readonly ColorTween _tweenMark, _tweenState;

            private Color _colorMarkOn, _colorMarkOff;
            private Color _targetColorMark, _targetColorState;

            public bool IsValid => Validate(_parent);

            public bool Value => throw new System.NotImplementedException();

            public SwitchEffect(VToggle parent)
            {
                _parent = parent;

            }

            public bool SetGraphic(Graphic checkmarkA, Graphic checkmarkB)
            {
                throw new System.NotImplementedException();
            }

            public void TransitionUpdate()
            {
                throw new System.NotImplementedException();
            }

            public void ColorsUpdate()
            {
                throw new System.NotImplementedException();
            }

            public void Play(bool isOn)
            {
                throw new System.NotImplementedException();
            }

            public void PlayInstant(bool isOn)
            {
                throw new System.NotImplementedException();
            }

            public void StateTransitionOn(Color targetColor, float duration, bool instant)
            {
                throw new System.NotImplementedException();
            }

            public void StateTransitionOff(Color targetColor, float duration, bool instant)
            {
                throw new System.NotImplementedException();
            }

          


        }
    }
}
