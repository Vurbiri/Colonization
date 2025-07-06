using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class SpellsSettings
    {
        [Header("Economic")]
        public int orderPerMana;

        public int blessBasa;
        public int blessPerRes;
        public int blessDuration;

        public int wrathBasa;
        public int wrathPerRes;
        public int wrathPierce;

        [Header("Military")]
        public int bloodTradePay;
        public int bloodTradeBay;

        public int healRandomValue;
        [Space]
        public IdArray<EconomicSpellId, int> economicCost;
        public IdArray<MilitarySpellId, int> militaryCost;
    }
}

