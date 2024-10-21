using UnityEngine;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBuildRoad : AButtonBuildType<LinkId>
    {
        private Players _players;
        private Player _playerCurrent;
        private Crossroad _currentCrossroad;

        public void Init(Players players, ACurrencies cost)
        {
            base.Init(Vector3.zero);
            _players = players;
            _cost = cost;
            _buttonClicked.AddListener(OnClick);
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public bool Setup(Crossroad crossroad, out CrossroadLink link)
        {
            link = crossroad.GetLink(_id);
            if (link == null)
            {
                _thisGO.SetActive(false);
                return false;
            }

            _playerCurrent = _players.Current;
            _currentCrossroad = crossroad;
            ACurrencies cash = _playerCurrent.Resources;
            int owner = link.Owner.Value;

            if (owner >= 0)
            {
                _button.Interactable = false;
                _targetGraphic.color = _players[owner].Color;
            }
            else
            {
                _button.Interactable = cash >= _cost;
                _targetGraphic.color = _playerCurrent.Color;
            }

            SetTextHint(_caption, cash, _cost);

            _thisGO.SetActive(true);
            return true;
        }

        private void OnClick()
        {
            _playerCurrent.BuyRoad(_currentCrossroad, _id);
        }
    }
}
