using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class SpellsSettings
    {
        [Range(2f, 12f)] public float nameShowTime;
        [Range(10f, 30f)] public float msgShowTime;
        [Space, Header("Order")]
        [Range(2, 12)] public int orderPerMana;
        [Header("RandomHealing")]
        public HitSFXName healSFX;
        [Range(10, 50)] public int healPercentValue;
        [Header("Blessing")]
        public HitSFXName blessSFX;
        [Range(4, 16)] public int blessBasa;
        [Range(2, 8)] public int blessPerRes;
        [Range(1, 3)] public int blessDuration;
        [Header("Wrath")]
        public HitSFXName wrathSFX;
        [Range(10, 50)] public int wrathBasa;
        [Range(5, 25)] public int wrathPerRes;
        [Range(10, 50)] public int wrathPierce;
        [Header("Sacrifice")]
        public HitSFXName sacrificeKnifeSFX;
        public HitSFXName sacrificeTargetSFX;
        [Range(50, 200)] public int sacrificeHPPercent;
        [Range(5, 25)] public int sacrificePierce;
        [Space]
        [Header("BloodTrade")]
        [Range(1, 3)] public int bloodTradePay;
        [Range(1, 3)] public int bloodTradeBay;
        [Header("Marauding")]
        [Range(10, 50)] public int reductionFromWall;
        [Header("SwapId")]
        public Color swapHexColor;
        [Range(2f, 12f)] public float swapShowTime;
        [Header("Zeal")]
        public HitSFXName zealSFX;
        [Range(1, 5)] public int zealAddAP;
        [Header("Cost")]
        public ReadOnlyIdArray<EconomicSpellId, int> economicCost;
        public ReadOnlyIdArray<MilitarySpellId, int> militaryCost;
        [Header("Hint")]
        public ReadOnlyIdArray<EconomicSpellId, string> economicKey;
        public ReadOnlyIdArray<MilitarySpellId, string> militaryKey;
    }
}

