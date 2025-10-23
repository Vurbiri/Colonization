using System;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBlock : AHintButton3D
    {
        public void Init(Action action)
        {
            base.InternalInit(GameContainer.UI.WorldHint, action, true);
        }

        public void Setup(Actor actor, ASkillUI skillUI)
        {
            _hintText = skillUI.GetText(interactable = actor.Action.CanUseSpecSkill());
            _thisGameObject.SetActive(true);
        }
    }
}
