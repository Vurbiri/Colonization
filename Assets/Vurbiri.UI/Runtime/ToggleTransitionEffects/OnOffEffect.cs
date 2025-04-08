//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\OnOffEffect.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        sealed private class OnOffEffect : TransitionEffect
        {
            internal static bool Validate(VToggle parent) => parent._checkmarkOn != null;

            public OnOffEffect(float duration, bool isOn, Graphic checkmark) : base(duration, isOn, checkmark, Color.white, new Color(1f, 1f, 1f, 0f))
            {
            }
        }
    }
}
