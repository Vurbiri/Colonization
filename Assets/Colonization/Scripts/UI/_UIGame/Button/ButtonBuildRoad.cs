//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonBuildRoad.cs
namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonBuildRoad : AButtonBuildType<LinkId>
    {
        public bool Setup(Crossroad crossroad, out CrossroadLink link)
        {
            link = crossroad.GetLink(_id);
            if (link == null || link.Owner.Value >= 0)
            {
                _thisGameObject.SetActive(false);
                return false;
            }

            Setup(crossroad);
            interactable = _cash >= _cost;

            _thisGameObject.SetActive(true);
            return true;
        }

        protected override void OnClick()
        {
            _parent.SetActive(false);
            _player.BuyRoad(_currentCrossroad, _id);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (string.Empty == _key)
                _key = "Road";
        }
#endif
    }
}
