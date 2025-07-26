using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class SpellsSettings
    {
        [Header("Order")]
        public int orderPerMana;
        [Header("Blessing")]
        public int blessBasa;
        public int blessPerRes;
        public int blessDuration;
        [Header("Wrath")]
        public int wrathBasa;
        public int wrathPerRes;
        public int wrathPierce;

        [Header("BloodTrade")]
        public int bloodTradePay;
        public int bloodTradeBay;
        [Header("HealRandom")]
        public int healRandomValue;
        [Header("Marauding")]
        public FileIdAndKey maraudingText;
        public int reductionFromWall;
        public IdArray<WarriorId, int> maraudingCount;
        [Header("SwapId")]
        public FileIdAndKey swapText;
        public Color swapHexColor;
        public float swapShowTime;
        [Space]
        public IdArray<EconomicSpellId, int> economicCost;
        public IdArray<MilitarySpellId, int> militaryCost;
    }
}

