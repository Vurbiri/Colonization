using System;

namespace Vurbiri.Colonization.UI
{
    public class SpecButton : AHintButton3D
    {
        public void Init(Action action)
        {
            base.InternalInit(action, true);
        }

        public void Setup(bool canUse, ASkillUI skillUI)
        {
            _hintText = skillUI.GetText(interactable = canUse);
            _thisGameObject.SetActive(true);
        }
    }
}
