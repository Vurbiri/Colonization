using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class SpellsSettings
    {
        // ---------- Economic ------------------
        public int orderPerMana;

        public int blessBasa;
        public int blessPerRes;
        public int blessDuration;

        // ---------- Military ------------------
        public int bloodTradePay;
        public int bloodTradeBay;

        public int healRandomValue;

        public IdArray<EconomicSpellId, int> economicCost;
        public IdArray<MilitarySpellId, int> militaryCost;
    }
}

