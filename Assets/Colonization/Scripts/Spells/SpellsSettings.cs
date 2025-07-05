using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class SpellsSettings
    {
        public int orderPerMana;

        public int bloodTradePay;
        public int bloodTradeBay;
        public int healRandomValue;

        public IdArray<EconomicSpellId, int> economicCost;
        public IdArray<MilitarySpellId, int> militaryCost;
    }
}

