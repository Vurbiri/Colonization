//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\ColorEffect.cs
namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        sealed private class ColorEffect : TransitionEffect
        {
            internal static bool Validate(VToggle parent) => ColorTween.Validate(parent._checkmarkOn, parent._isFade, parent._fadeDuration);

            public ColorEffect(VToggle parent) : base(parent, parent._isOn, parent._checkmarkOn, parent._colorOn, parent._colorOff)
            {
            }

            public override void ColorsUpdate() 
            {
                _colorMarkOn = _parent._colorOn;
                _colorMarkOff = _parent._colorOff;
                _targetColorMark = _parent._isOn ? _colorMarkOn : _colorMarkOff;
            }

        }
}
}
