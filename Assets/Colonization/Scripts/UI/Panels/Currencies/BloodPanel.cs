using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class BloodPanel : ASinglyPanel<CurrentMaxPopup>
    {
        public void Init(Direction2 directionPopup, ACurrenciesReactive currencies, ProjectColors colors, CanvasHint hint)
        {
            _widget.Init(currencies.CurrentBlood, currencies.MaxBlood, colors, directionPopup, hint);

            Destroy(this);
        }
    }
}
