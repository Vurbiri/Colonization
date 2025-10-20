using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class CasterSettings
	{
        public int percentAmountOffset;
        [Header("Order & Blessing & Wrath")]
        public int resDivider;
        public float maxUseRes;
        [Header("Transmutation")]
        public int minRes;
        [Header("Sacrifice")]
        public int skillId;
        public WaitRealtime waitBeforeSelecting;
        [Header("Weights")]
        public ReadOnlyIdArray<EconomicSpellId, int> weightsEconomic;
        public ReadOnlyIdArray<MilitarySpellId, int> weightsMilitary;
    }
}
