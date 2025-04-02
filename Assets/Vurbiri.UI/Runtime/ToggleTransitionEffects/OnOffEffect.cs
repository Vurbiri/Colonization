//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\OnOffEffect.cs
using UnityEngine;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        sealed private class OnOffEffect : TransitionEffect
        {
            internal static bool Validate(VToggle parent)
            {
                if (parent._checkmarkOn == null) return false;

                return parent._markTransition == ToggleTransition.Instant | !Mathf.Approximately(parent._transitionDuration, 0);
            }

            public override bool IsValid => Validate(_parent);

            public OnOffEffect(VToggle parent) : base(parent)
            {
                _colorMarkOn = Color.white;
                _colorMarkOff = new Color(1f, 1f, 1f, 0f);
                _targetColorMarkA = _parent._isOn ? _colorMarkOn : _colorMarkOff;
                PlayInstant();
            }


        }
    }
}
