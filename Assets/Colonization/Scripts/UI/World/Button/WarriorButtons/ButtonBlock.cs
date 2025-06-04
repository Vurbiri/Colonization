using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBlock : AWorldHintButton
    {
        public void Init(WorldHint hint, Action action)
        {
            base.Init(hint, action, true);
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
