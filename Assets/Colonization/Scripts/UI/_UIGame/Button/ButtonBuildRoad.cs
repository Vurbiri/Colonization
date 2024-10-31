namespace Vurbiri.Colonization.UI
{
    public class ButtonBuildRoad : AButtonBuildType<LinkId>
    {
        private Player _playerCurrent;
        private Crossroad _currentCrossroad;

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
                _targetGraphic.color = _players[owner].Visual.color;
            }
            else
            {
                _button.Interactable = cash >= _cost;
                _targetGraphic.color = _playerCurrent.Visual.color;
            }

            SetTextHint(_caption, cash, _cost);

            _thisGO.SetActive(true);
            return true;
        }

        protected override void OnClick()
        {
            _parentGO.SetActive(false);
            _playerCurrent.BuyRoad(_currentCrossroad, _id);
        }
    }
}
