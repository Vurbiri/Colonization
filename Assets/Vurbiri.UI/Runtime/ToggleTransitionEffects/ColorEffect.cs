//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\ColorEffect.cs
namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        sealed private class ColorEffect : TransitionEffect
        {
            private readonly VToggle _parent;

            internal static bool Validate(VToggle parent) => parent._checkmarkOn != null;

            public ColorEffect(VToggle parent) : base(parent._fadeDuration, parent._isOn, parent._checkmarkOn, parent._colorOn, parent._colorOff)
            {
                _parent = parent;
            }

            public override void ColorsUpdate() 
            {
                _colorMarkOn = _parent._colorOn;
                _colorMarkOff = _parent._colorOff;
            }
        }
}
}
