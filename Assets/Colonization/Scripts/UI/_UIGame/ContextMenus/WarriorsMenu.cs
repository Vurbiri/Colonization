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
        [SerializeField] private ButtonSkill[] _buttonsSkills;

        private int _countButtonsAttack;
        private Players _players;
        private Actor _currentWarrior;

        private readonly Vector3[][] _buttonPositions = new Vector3[4][];

        public void Init(ContextMenuSettings settings)
        {
            _players = settings.players;

            _countButtonsAttack = _buttonsSkills.Length;
            float angle;
            Vector3 distance = new(0f, _distanceOfButtons, 0f);
            for (int i = 0; i <= _countButtonsAttack; i++)
            {
                angle = 180f / (i + 1);
                _buttonPositions[i] = new Vector3[i];
                for (int k = 0; k < i; k++)
                    _buttonPositions[i][k] = Quaternion.Euler(0f, 0f, angle * (k + 1)) * distance;
            }

            _buttonClose.Init(settings.hint, OnClose);

            _buttonMovement.Init(-distance, settings.hint, settings.color, OnMovement);

            
            for (int i = 0; i < _countButtonsAttack; i++)
                _buttonsSkills[i].Init(settings, _thisGO);

            _thisGO.SetActive(false);
        }

        public void Open(Actors.Actor actor)
        {
            _currentWarrior = actor;
            
            _buttonMovement.Setup(true, _currentWarrior.CanMove());

            var skills = _warriorsSettings[actor.Id].Skills.SkillsUI;
            int count = skills.Count, index;
            
            for (index = 0; index < count; index++)
                _buttonsSkills[index].Setup(actor, index, skills[index], _buttonPositions[count][index]);

            for (; index < _countButtonsAttack; index++)
                _buttonsSkills[index].Disable();

            _thisGO.SetActive(true);
        }

        private void OnMovement()
        {
            _thisGO.SetActive(false);
            _currentWarrior.Move();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_buttonsSkills == null || _buttonsSkills.Length == 0)
                _buttonsSkills = GetComponentsInChildren<ButtonSkill>();
            if (_warriorsSettings == null)
                _warriorsSettings = VurbiriEditor.Utility.FindAnyScriptable<WarriorsSettingsScriptable>();
        }
#endif
    }
}
