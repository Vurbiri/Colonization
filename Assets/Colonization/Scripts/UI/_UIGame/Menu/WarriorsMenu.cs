using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class WarriorsMenu : AWorldMenu
    {
        [SerializeField] protected float _distanceOfButtons = 5f;
        [Space]
        [SerializeField] private CmButton _buttonClose;

        private Players _players;
        private Player _playerCurrent;

        public void Init(Players players, PricesScriptable prices)
        {
            _players = players;


            _buttonClose.onClick.AddListener(OnClose);

            _thisGO.SetActive(false);
        }

        public void Open(Warrior Warrior)
        {
            _thisGO.SetActive(true);
        }
    }
}
