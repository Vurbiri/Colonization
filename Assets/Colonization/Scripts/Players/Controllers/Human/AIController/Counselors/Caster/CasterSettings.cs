using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class CasterSettings
	{
        public int spellDivider;
        public int percentAmountOffset;
        public WaitRealtime waitBeforeSelecting;
        [Header("Order & Blessing & Wrath")]
        public float useResRatio;
        [Header("Sacrifice")]
        public int skillId;
        [Header("SwapId")]
        public ReadOnlyArray<int> goodIds;
        public ReadOnlyArray<int> badIds;
        [Header("Weights")]
        public ReadOnlyIdArray<EconomicSpellId, int> weightsEconomic;
        public ReadOnlyIdArray<MilitarySpellId, int> weightsMilitary;
    }
}
