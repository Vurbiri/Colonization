//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonBlock.cs
using UnityEngine;
using UnityEngine.Events;
using Vurbiri.Colonization.Actors;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBlock : AHintingButton
    {
        public void Init(Vector3 localPosition, HintGlobal hint, Color color, UnityAction action)
        {
            base.Init(localPosition, hint, action, true);
            _button.targetGraphic.color = color;
        }

        public void Setup(Actor actor, BlockUI blockUI)
        {
            bool isUse = actor.ActionPoint >= blockUI.Cost;

            _button.Interactable = isUse;
            _text = blockUI.GetText(isUse);

            _thisGO.SetActive(true);
        }
    }
}
