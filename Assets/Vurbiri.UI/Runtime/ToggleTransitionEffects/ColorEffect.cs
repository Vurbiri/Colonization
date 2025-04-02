//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\ColorEffect.cs
using UnityEngine;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        sealed private class ColorEffect : TransitionEffect
        {
            internal static bool Validate(VToggle parent)
            {
                if (parent._checkmarkOn == null) return false;

                return parent._markTransition == ToggleTransition.Instant | !Mathf.Approximately(parent._transitionDuration, 0);
            }

            public override bool IsValid => Validate(_parent);
            public override bool Value => _parent._checkmarkOn.canvasRenderer.GetColor() == TargetColorA;

            public ColorEffect(VToggle parent) : base(parent)
            {
                ColorsUpdate();
                PlayInstant();
            }

            public override void ColorsUpdate() 
            {
                _colorMarkOn = _parent._colorOn;
                _colorMarkOff = _parent._colorOff;
                _targetColorMarkA = _parent._isOn ? _colorMarkOn : _colorMarkOff;
            }

        }
}
}
