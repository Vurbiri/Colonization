//Assets\Colonization\Scripts\UI\_UIGame\Button\WarriorButtons\ButtonSkill.cs
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Actors;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonSkill : AHintingButton
    {
        [Space]
        [SerializeField] private Image _iconImage;

        private GameObject _parentGO;
        private Actor _currentActor;
        private int _idSkill;

        public virtual void Init(ButtonSettings settings, GameObject parent)
        {
            Init(settings.hint, OnClick, false);

            _button.targetGraphic.color = settings.color;
            _parentGO = parent;
        }

        public void Setup(Actor actor, int idSkill, SkillUI skillUI, Vector3 localPosition)
        {
            bool isUse = actor.ActionPoint >= skillUI.Cost;

            _thisTransform.localPosition = localPosition;

            _currentActor = actor;
            _idSkill = idSkill;

            _iconImage.sprite = skillUI.Sprite;
            _button.Interactable = isUse;

            _text = skillUI.GetText(isUse);

            _thisGO.SetActive(true);
        }

        public void Disable() => _thisGO.SetActive(false);

        private void OnClick()
        {
            _parentGO.SetActive(false);
            _currentActor.UseSkill(_idSkill);
        }
    }
}
