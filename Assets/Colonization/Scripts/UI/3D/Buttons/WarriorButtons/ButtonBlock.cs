using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBlock : AHintButton3D
    {
        public void Init(Action action)
        {
            base.Init(GameContainer.UI.WorldHint, action, true);
        }

        public void Setup(Actor actor, ASkillUI skillUI)
        {
            _text = skillUI.GetText(interactable = actor.ActionPoint >= skillUI.Cost);
            _thisGameObject.SetActive(true);
        }
    }
}
