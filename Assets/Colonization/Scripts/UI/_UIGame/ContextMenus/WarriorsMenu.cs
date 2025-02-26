//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\WarriorsMenu.cs
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class WarriorsMenu : AWorldMenu
    {
        [SerializeField] private float _distanceOfButtons = 5f;
        [Space]
        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;
        [Space]
        [SerializeField] private HintingButton _buttonClose;
        [SerializeField] private HintingButton _buttonMovement;
        [SerializeField] private ButtonBlock _buttonBlock;
        [SerializeField] private ButtonSkill[] _buttonsSkill;

        private int _countButtonsSkill;
        private Actor _currentWarrior;

        private Vector3[][] _buttonPositions;

        public void Init(ContextMenuSettings settings)
        {
            CreatePositionButtons();
            Vector3 distance = new(0f, _distanceOfButtons, 0f);

            _buttonClose.Init(settings.hint, OnClose);
            _buttonMovement.Init(-distance, settings.hint, settings.playerColor, OnMovement);
            _buttonBlock.Init(distance, settings.hint, settings.playerColor, OnBlock);

            for (int i = 0; i < _countButtonsSkill; i++)
                _buttonsSkill[i].Init(settings, _thisGO);

            _thisGO.SetActive(false);
        }

        public void Open(Actors.Actor actor)
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

            _thisGO.SetActive(true);
        }

        private void OnMovement()
        {
            _thisGO.SetActive(false);
            _currentWarrior.Move();
        }

        private void OnBlock()
        {
            _thisGO.SetActive(false);
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
