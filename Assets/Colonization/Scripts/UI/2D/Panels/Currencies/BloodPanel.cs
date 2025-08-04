namespace Vurbiri.Colonization.UI
{
    sealed public class BloodPanel : ASinglyPanel<CurrentMaxPopup>
    {
        public void Init(Direction2 directionPopup, ACurrenciesReactive currencies)
        {
            _widget.Init(currencies.Get(CurrencyId.Blood), currencies.MaxBlood, directionPopup);

            Destroy(this);
        }
    }
}
