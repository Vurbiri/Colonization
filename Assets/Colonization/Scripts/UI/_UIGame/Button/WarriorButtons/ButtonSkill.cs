//Assets\Colonization\Scripts\UI\_UIGame\Button\WarriorButtons\ButtonSkill.cs
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Actors;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonSkill : AWorldHintButton
    {
        [Space]
        [SerializeField] private Image _iconImage;
        [Space]
        [SerializeField] private int _indexApplyColor;

        private AWorldMenu _parent;
        private Actor _currentActor;
        private int _idSkill;

        public void Init(ButtonSettings settings, AWorldMenu parent)
        {
            Init(settings.hint, OnClick, false);

            _targetGraphics[_indexApplyColor].SetGraphicColor(settings.playerColor);
            _parent = parent;
        }

        public void Setup(Actor actor, int idSkill, SkillUI skillUI, Vector3 localPosition)
        {
            bool isUse = actor.ActionPoint >= skillUI.Cost;

            _thisTransform.localPosition = localPosition;

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
