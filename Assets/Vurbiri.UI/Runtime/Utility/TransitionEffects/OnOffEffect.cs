//Assets\Vurbiri.UI\Runtime\Utility\TransitionEffects\OnOffEffect.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public class OnOffEffect : TransitionEffect
    {
        public OnOffEffect(float duration, bool isOn, Graphic checkmark) : base(duration, isOn, checkmark, Color.white, new Color(1f, 1f, 1f, 0f))
        {
        }
    }
}
