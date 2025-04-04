//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\OnOffEffect.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        sealed private class OnOffEffect : TransitionEffect
        {
            internal static bool Validate(VToggle parent) => ColorTween.Validate(parent._checkmarkOn, parent._isFade, parent._fadeDuration);

            public OnOffEffect(VToggle parent, bool isOn, Graphic checkmark) : base(parent, isOn, checkmark, Color.white, new Color(1f, 1f, 1f, 0f))
            {
            }
        }
    }
}
