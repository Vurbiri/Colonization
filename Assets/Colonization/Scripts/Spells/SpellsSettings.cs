using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class SpellsSettings
    {
        [Header("Order")]
        public int orderPerMana;
        [Header("RandomHealing")]
        public HitSFXName healSFX;
        public int healPercentValue;
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
        public HitSFXName sacrificeKnifeSFX;
        public HitSFXName sacrificeTargetSFX;
        public int sacrificeBloodCost;
        public int sacrificeHPPercent;
        public int sacrificePierce;
        [Space]
        [Header("BloodTrade")]
        public int bloodTradePay;
        public int bloodTradeBay;
        [Header("Marauding")]
        public int reductionFromWall;
        public IdArray<WarriorId, int> maraudingCount;
        [Header("SwapId")]
        public Color swapHexColor;
        public float swapShowTime;
        [Header("Zeal")]
        public HitSFXName zealSFX;
        public int zealPercentHeal;
        public int zealAddAP;
        [Header("Cost")]
        public ReadOnlyIdArray<EconomicSpellId, int> economicCost;
        public ReadOnlyIdArray<MilitarySpellId, int> militaryCost;
        [Header("Hint")]
        public IdArray<EconomicSpellId, string> economicKey;
        public IdArray<MilitarySpellId, string> militaryKey;
    }
}

