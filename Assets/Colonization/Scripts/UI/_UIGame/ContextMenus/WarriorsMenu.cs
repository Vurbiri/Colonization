//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\WarriorsMenu.cs
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class WarriorsMenu : AWorldMenu
    {
        [SerializeField] private float _distanceOfButtons = 5f;
        [Space]
        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;
        [Space]
        [SerializeField] private WorldHintButton _buttonClose;
        [Space]
        [SerializeField] private WorldHintButton _buttonMovement;
        [SerializeField] private ButtonBlock _buttonBlock;
        [SerializeField] private ButtonSkill[] _buttonsSkill;

        private int _countButtonsSkill;
        private Actor _currentWarrior;

        private Vector3[][] _buttonPositions;

        public ISigner<GameObject, bool> Init(ContextMenuSettings settings)
        {
            CreatePositionButtons();
            Vector3 distance = new(0f, _distanceOfButtons, 0f);

            _buttonClose.Init(settings.hint, OnClose);
            _buttonMovement.Init(-distance, settings.hint, settings.playerColor, OnMovement);
            _buttonBlock.Init(distance, settings.hint, settings.playerColor, OnBlock);

            for (int i = 0; i < _countButtonsSkill; i++)
                _buttonsSkill[i].Init(settings, this);

            CloseInstant();
            
            return _eventActive;
        }

        public void Open(Actor actor)
        {
            _currentWarrior = actor;
            var skills = _warriorsSettings[actor.Id].Skills;

            _buttonMovement.Setup(true, _currentWarrior.CanMove);
            _buttonBlock.Setup(actor, skills.BlockUI);

            var skillsUI = skills.SkillsUI;
            int count = skillsUI.Count, index = 0;
            
            for (; index < count; index++)
                _buttonsSkill[index].Setup(actor, index, skillsUI[index], _buttonPositions[count][index]);

            for (; index < _countButtonsSkill; index++)
                _buttonsSkill[index].Disable();

            Open();
        }

        private void OnMovement()
        {
            Close();
            _currentWarrior.Move();
        }

        public void OnBlock()
        {
            Close();
            _currentWarrior.Block();
        }

        private void CreatePositionButtons()
        {
            _countButtonsSkill = _buttonsSkill.Length;
            _buttonPositions = new Vector3[_countButtonsSkill + 1][];
            float angle;
            Vector3 distance = new(0f, _distanceOfButtons, 0f);
            for (int i = 0, j, right, left; i <= _countButtonsSkill; i++)
            {
                _buttonPositions[i] = new Vector3[i];
                left = i >> 1; right = i - left;

                angle = 180f / (left + 1);
                for (j = 0; j < left; j++)
                    _buttonPositions[i][j] = Quaternion.Euler(0f, 0f, angle * (j + 1)) * distance;

                angle = -180f / (right + 1);
                for (; j < i; j++)
                    _buttonPositions[i][j] = Quaternion.Euler(0f, 0f, angle * (right--)) * distance;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_buttonsSkill == null || _buttonsSkill.Length == 0)
                _buttonsSkill = GetComponentsInChildren<ButtonSkill>();
            if (_warriorsSettings == null)
                _warriorsSettings = EUtility.FindAnyScriptable<WarriorsSettingsScriptable>();
            if (_buttonBlock == null)
                _buttonBlock = GetComponentInChildren<ButtonBlock>();
        }
#endif
    }
}
