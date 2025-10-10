using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class BuilderSettings
	{
        public int profitWeight;
        public int costWeight;
        [Space]
        public ReadOnlyIdArray<EdificeId, int> edificeWeight;
        public int wallWeight;
        [Header("LandBuild")]
        public int searchDepth;
        public int penaltyPerRoad;
    }
}
