using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Prices", menuName = "Vurbiri/Colonization/Prices", order = 51)]
    public class PricesScriptable : ScriptableObject
    {
        [SerializeField] private CurrenciesLite _playersDefault;
        [SerializeField] private CurrenciesLite _roads;

        [SerializeField] private CurrenciesLite _portOne;
        [SerializeField] private CurrenciesLite _portTwo;
        [SerializeField] private CurrenciesLite _lighthouseOne;
        [SerializeField] private CurrenciesLite _lighthouseTwo;

        public CurrenciesLite PlayersDefault => _playersDefault;
        public CurrenciesLite Roads => _roads;

        public CurrenciesLite PortOne => _portOne;
        public CurrenciesLite PortTwo => _portTwo;
        public CurrenciesLite LighthouseOne => _lighthouseOne;
        public CurrenciesLite LighthouseTwo => _lighthouseTwo;
    }
}
