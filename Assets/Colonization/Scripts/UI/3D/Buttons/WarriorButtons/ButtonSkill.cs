using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Actors;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonSkill : AHintButton3D
    {
        [Space]
        [SerializeField] private Image _iconImage;

        private AWorldMenu _parent;
        private Actor _currentActor;
        private int _idSkill;

        public void Init(ButtonSettings settings, AWorldMenu parent)
        {
            Init(settings.hint, OnClick, false);

            _parent = parent;
        }

        public void Setup(Actor actor, int idSkill, SkillUI skillUI, Vector3 localPosition)
        {
            bool isUse = actor.ActionPoint >= skillUI.Cost;

            _rectTransform.localPosition = localPosition;

            _currentActor = actor;
            _idSkill = idSkill;

            _iconImage.sprite = skillUI.Sprite;
            interactable = isUse;

            _text = skillUI.GetText(isUse);

            _thisGameObject.SetActive(true);
        }

        public void Disable() => _thisGameObject.SetActive(false);

        private void OnClick()
        {
            _parent.Close();
            _currentActor.UseSkill(_idSkill);
        }
    }
}
