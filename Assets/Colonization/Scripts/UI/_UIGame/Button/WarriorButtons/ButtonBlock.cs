//Assets\Colonization\Scripts\UI\_UIGame\Button\WarriorButtons\ButtonBlock.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBlock : AWorldHintButton
    {
        [SerializeField] private int _indexApplyColor;

        public void Init(Vector3 localPosition, WorldHint hint, Color color, Action action)
        {
            base.Init(localPosition, hint, action, true);

            _targetGraphics[_indexApplyColor].SetGraphicColor(color);
        }

        public void Setup(Actor actor, BlockUI blockUI)
        {
            bool isUse = actor.ActionPoint >= blockUI.Cost;

            interactable = isUse;
            _text = blockUI.GetText(isUse);

            _thisGameObject.SetActive(true);
        }
    }
}
