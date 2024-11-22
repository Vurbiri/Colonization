//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonBuildRoad.cs
namespace Vurbiri.Colonization.UI
{
    public class ButtonBuildRoad : AButtonBuildType<LinkId>
    {
        private Player _playerCurrent;
        private Crossroad _currentCrossroad;

        public bool Setup(Crossroad crossroad, out CrossroadLink link)
        {
            link = crossroad.GetLink(_id);
            if (link == null || link.Owner.Value >= 0)
            {
                _thisGO.SetActive(false);
                return false;
            }

            _playerCurrent = _players.Current;
            _currentCrossroad = crossroad;
            ACurrencies cash = _playerCurrent.Resources;

            _button.Interactable = cash >= _cost;

            SetTextHint(_caption, cash, _cost);

            _thisGO.SetActive(true);
            return true;
        }

        protected override void OnClick()
        {
            _parentGO.SetActive(false);
            _playerCurrent.BuyRoad(_currentCrossroad, _id);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (string.Empty == _key)
                _key = "Road";
        }
#endif
    }
}
