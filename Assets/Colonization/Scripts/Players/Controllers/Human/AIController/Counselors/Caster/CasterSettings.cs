using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class CasterSettings
	{
        
        [Header("Order & Blessing & Wrath")]
        public int resDivider;
        public int maxUseRes;
        [Header("Weights")]
        public ReadOnlyIdArray<EconomicSpellId, int> weightsEconomic;
        public ReadOnlyIdArray<MilitarySpellId, int> weightsMilitary;
    }
}
