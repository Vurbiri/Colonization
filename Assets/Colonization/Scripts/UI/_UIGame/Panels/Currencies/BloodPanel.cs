//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\BloodPanel.cs
namespace Vurbiri.Colonization.UI
{
    sealed public class BloodPanel : ASinglyPanel<CurrentMaxPopup>
    {
        public void Init(Direction2 directionPopup, ACurrenciesReactive currencies, ProjectColors colors)
        {
            _widget.Init(currencies.BloodCurrent, currencies.BloodMax, colors, directionPopup);

            Destroy(this);
        }
    }
}
