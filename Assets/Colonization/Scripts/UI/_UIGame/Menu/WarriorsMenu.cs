using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Localization;
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
        [SerializeField] private ButtonAttack[] _buttonsAttack;

        private int _countButtonsAttack;
        private Players _players;
        private Actor _currentWarrior;

        private readonly Vector3[][] _buttonPositions = new Vector3[4][];

        public void Init(Players players, Color color)
        {
            _players = players;
            Language language = SceneServices.Get<Language>();

            _countButtonsAttack = _buttonsAttack.Length;
            float angle;
            Vector3 distance = new(0f, _distanceOfButtons, 0f);
            for (int i = 0; i <= _countButtonsAttack; i++)
            {
                angle = 180f / (i + 1);
                _buttonPositions[i] = new Vector3[i];
                for (int k = 0; k < i; k++)
                    _buttonPositions[i][k] = Quaternion.Euler(0f, 0f, angle * (k + 1)) * distance;
            }

            _buttonClose.Init(OnClose);

            _buttonMovement.Init(-distance, color, OnMovement);

            
            for (int i = 0; i < _countButtonsAttack; i++)
                _buttonsAttack[i].Init(color, _thisGO, language);

            _thisGO.SetActive(false);
        }

        public void Open(Actors.Actor actor)
        {
            _currentWarrior = actor;
            
            _buttonMovement.Setup(_currentWarrior.CanMove());

            List<SkillUI> settings = _warriorsSettings[actor.Id].Skills.GetAttackSkillsUI();
            int count = settings.Count, index;
            
            for (index = 0; index < count; index++)
                _buttonsAttack[index].Setup(actor, index, settings[index], _buttonPositions[count][index]);

            for (; index < _countButtonsAttack; index++)
                _buttonsAttack[index].Disable();

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
            if(_buttonsAttack == null || _buttonsAttack.Length == 0)
                _buttonsAttack = GetComponentsInChildren<ButtonAttack>();
            if (_warriorsSettings == null)
                _warriorsSettings = VurbiriEditor.Utility.FindAnyScriptable<WarriorsSettingsScriptable>();
        }
#endif
    }
}
