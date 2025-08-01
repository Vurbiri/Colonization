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
        public HitSFXName blessSFX;
        public int blessBasa;
        public int blessPerRes;
        public int blessDuration;
        [Header("Wrath")]
        public HitSFXName wrathSFX;
        public int wrathBasa;
        public int wrathPerRes;
        public int wrathPierce;
        [Header("Sacrifice")]
        public FileIdAndKey sacrificeText;
        public HitSFXName sacrificeKnifeSFX;
        public HitSFXName sacrificeTargetSFX;
        public int sacrificeHPPercent;
        public int sacrificePierce;
        [Space]
        [Header("BloodTrade")]
        public int bloodTradePay;
        public int bloodTradeBay;
        [Header("Marauding")]
        public FileIdAndKey maraudingText;
        public int reductionFromWall;
        public IdArray<WarriorId, int> maraudingCount;
        [Header("SwapId")]
        public FileIdAndKey swapText;
        public Color swapHexColor;
        public float swapShowTime;
        [Header("Zeal")]
        public int zealPercentHeal;
        public int zealAddAP;
        [Space(15f)]
        public IdArray<EconomicSpellId, int> economicCost;
        public IdArray<MilitarySpellId, int> militaryCost;
    }
}

