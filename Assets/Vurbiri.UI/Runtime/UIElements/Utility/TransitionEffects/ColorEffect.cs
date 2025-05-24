using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public class ColorEffect : TransitionEffect
    {
        public ColorEffect(float duration, bool isOn, Graphic checkmark, Color colorMarkOn, Color colorMarkOff)
            : base(duration, isOn, checkmark, colorMarkOn, colorMarkOff)
        {

        }

        public override void ColorsUpdate(Color colorOn, Color colorOff)
        {
            _colorMarkOn = colorOn;
            _colorMarkOff = colorOff;
        }
    }
}
