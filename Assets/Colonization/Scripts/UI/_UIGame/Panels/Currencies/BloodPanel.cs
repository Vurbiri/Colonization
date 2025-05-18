//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\BloodPanel.cs
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class BloodPanel : ASinglyPanel<CurrentMaxPopup>
    {
        public void Init(Direction2 directionPopup, ACurrenciesReactive currencies, ProjectColors colors, CanvasHint hint)
        {
            _widget.Init(currencies.BloodCurrent, currencies.BloodMax, colors, directionPopup, hint);

            Destroy(this);
        }
    }
}
