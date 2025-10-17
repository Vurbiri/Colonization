using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class CasterSettings
	{
        [Header("Order")]
        public int maxMana;
        [Header("Weights")]
        public ReadOnlyIdArray<EconomicSpellId, int> weightsEconomic;
        public ReadOnlyIdArray<MilitarySpellId, int> weightsMilitary;
    }
}
