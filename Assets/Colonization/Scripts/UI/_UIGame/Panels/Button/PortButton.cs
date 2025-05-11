//Assets\Colonization\Scripts\UI\_UIGame\Panels\Button\PortButton.cs
using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.UI
{
    sealed public class PortButton : AEdificeButton
    {

        public override void Init(Crossroad crossroad, InputController inputController, int index, Sprite sprite, bool isOn)
        {
            base.Init(crossroad, inputController, index, sprite, isOn);

        }

        public override void OnChange(Crossroad crossroad, Sprite sprite)
        {
            _icon.sprite = sprite;
        }
    }
}
