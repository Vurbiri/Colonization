using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class WarriorsMenu : AWorldMenu
    {
        [SerializeField] protected float _distanceOfButtons = 5f;
        [Space]
        [SerializeField] private HintingButton _buttonClose;
        [SerializeField] private HintingButton _buttonMovement;

        private Players _players;
        private Actors.Warrior _currentWarrior;

        public void Init(Players players)
        {
            _players = players;

            _buttonClose.Init(Vector3.zero, OnClose);

            _buttonMovement.Init(new(0f, _distanceOfButtons, 0f), OnMovement);

            _thisGO.SetActive(false);
        }

        public void Open(Actors.Warrior warrior)
        {
            _currentWarrior = warrior;
            Color currentColor = _players.Current.Visual.color;

            _buttonMovement.Setup(_currentWarrior.CanMove(), currentColor);


            _thisGO.SetActive(true);
        }

        private void OnMovement()
        {
            _thisGO.SetActive(false);
            _currentWarrior.Move();
        }
    }
}
