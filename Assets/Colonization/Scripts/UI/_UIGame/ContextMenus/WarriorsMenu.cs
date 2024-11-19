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
        [SerializeField] private ButtonSkill[] _buttonsSkill;

        private int _countButtonsSkill;
        private Players _players;
        private Actor _currentWarrior;

        private Vector3[][] _buttonPositions;

        public void Init(ContextMenuSettings settings)
        {
            _players = settings.players;

            CreatePositionButtons();
            Vector3 distance = new(0f, _distanceOfButtons, 0f);

            _buttonClose.Init(settings.hint, OnClose);
            _buttonMovement.Init(-distance, settings.hint, settings.color, OnMovement);

            for (int i = 0; i < _countButtonsSkill; i++)
                _buttonsSkill[i].Init(settings, _thisGO);

            _thisGO.SetActive(false);
        }

        public void Open(Actors.Actor actor)
        {
            _currentWarrior = actor;
            var warriorSettings = _warriorsSettings[actor.Id];

            _buttonMovement.Setup(true, _currentWarrior.CanMove());


            var skills = warriorSettings.Skills.SkillsUI;
            int count = skills.Count, index;
            
            for (index = 0; index < count; index++)
                _buttonsSkill[index].Setup(actor, index, skills[index], _buttonPositions[count][index]);

            for (; index < _countButtonsSkill; index++)
                _buttonsSkill[index].Disable();

            _thisGO.SetActive(true);
        }

        private void OnMovement()
        {
            _thisGO.SetActive(false);
            _currentWarrior.Move();
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
                right = i >> 1; left = i - right;

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
                _warriorsSettings = VurbiriEditor.Utility.FindAnyScriptable<WarriorsSettingsScriptable>();
        }
#endif
    }
}