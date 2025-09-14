namespace Vurbiri.Colonization.UI
{
    sealed public class ScorePanel : ASinglyPanel<ScorePopup>
    {
        public void Init(Direction2 directionPopup)
        {
            _widget.Init(directionPopup);
            Destroy(this);
        }
    }
}
