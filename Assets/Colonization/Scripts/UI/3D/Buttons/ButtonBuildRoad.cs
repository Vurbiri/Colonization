namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonBuildRoad : AButtonBuildType<LinkId>
    {
        public bool Setup(Crossroad crossroad, out CrossroadLink link)
        {
            link = crossroad.Links[_id];
            if (link == null)
            {
                _thisGameObject.SetActive(false);
                return false;
            }

            Setup(crossroad);
            InteractableAndUnlock(_cash >= _cost, link.IsEmpty);

            _thisGameObject.SetActive(true);
            return true;
        }

        protected override void OnClick()
        {
            _parent.Close();
            GameContainer.Players.Person.BuyRoad(_currentCrossroad, _id);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_getText.key == string.Empty)
            {
                _getText.id = LangFiles.Gameplay;
                _getText.key = "Road";
            }
        }
#endif
    }
}
