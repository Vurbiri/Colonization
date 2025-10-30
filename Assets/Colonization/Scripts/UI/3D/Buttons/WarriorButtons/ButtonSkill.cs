using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonSkill : AHintButton3D
    {
        [Space]
        [SerializeField] private Image _iconImage;

        private AWorldMenu _parent;
        private Actor _currentActor;
        private int _idSkill;

        public void Init(AWorldMenu parent)
        {
            InternalInit(OnClick, false);

            _parent = parent;
        }

        public void Setup(Actor actor, int idSkill, SkillUI skillUI, Vector3 localPosition)
        {
            bool isUse = actor.Action.CanUseSkill(idSkill);

            _thisRectTransform.localPosition = localPosition;

            _currentActor = actor;
            _idSkill = idSkill;

            _iconImage.sprite = skillUI.Sprite;
            interactable = isUse;

            _hintText = skillUI.GetText(isUse);

            _thisGameObject.SetActive(true);
        }

        public void Disable() => _thisGameObject.SetActive(false);

        private void OnClick()
        {
            _parent.Close();
            _currentActor.Action.UseSkill(_idSkill);
        }
    }
}
