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
        [SerializeField] private HintingButton _buttonAttack;

        private Players _players;
        private Actors.Actor _currentWarrior;

        public void Init(Players players, Color color)
        {
            _players = players;

            _buttonClose.Init(Vector3.zero, OnClose);

            _buttonMovement.Init(new(0f, -_distanceOfButtons, 0f), color, OnMovement);
            _buttonAttack.Init(new(0f, _distanceOfButtons, 0f), color, OnAttack);

            _thisGO.SetActive(false);
        }

        public void Open(Actors.Actor warrior)
        {
            _currentWarrior = warrior;
            

            _buttonMovement.Setup(_currentWarrior.CanMove());


            _thisGO.SetActive(true);
        }

        private void OnMovement()
        {
            _thisGO.SetActive(false);
            _currentWarrior.Move();
        }

        private void OnAttack()
        {
            _thisGO.SetActive(false);
            _currentWarrior.Attack();
        }
    }
}
